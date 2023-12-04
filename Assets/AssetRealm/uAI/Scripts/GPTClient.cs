using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using System;

namespace UAI{   
    public class GPTClient 
    { 
        private static GPTClient _instance;
        public static GPTClient Instance {
            get {   
                if (null == _instance)
                {
                    _instance = new GPTClient();
                } 
                #if UNITY_EDITOR 
                    _instance.loadSavedConfiguration();
                #endif
                return _instance;
            }
        }
    
        public GPTClient()
        { 
            _instance = this; 
        }

        public static string[] models = new string[] { "gpt-3.5-turbo", "gpt-3.5-turbo-16k", "gpt-3.5-turbo-1106",  "gpt-4", "gpt-4-1106-preview" };
        //hashmap model and cost
        public static Dictionary<string, float> modelCosts = new Dictionary<string, float>( );
        public static int modelIndex = 0;
        public static bool showSettings = false;

        public string apiKey = "";  
        private string apiUrl = "https://api.openai.com/v1/chat/completions";
        
        public string model = "gpt-3.5-turbo";

        [HideInInspector]
        public static GPTStatus status = GPTStatus.Idle;
        [HideInInspector]
        public string SystemInitPrompt = "You are a professional programmer.";

        public static float temperature = 0.7f;
        public static int maxTokens = 3000;

        public static string defaultSavePath = "Assets/";
        public static bool askForSavePath = true;
        public static int n = 1;

        public static float cost = 0f;
   

        public string response = "";
        
        public Action<string, int> OnResponseReceived; 
        public Action<string> OnPartResponseReceived; 
        
        public static UnityWebRequest currentRequest;
        string reponseText = ""; 
        int lastAmount = 0;

        public void SendRequest(string promptSend,   int index = 0)
        { 
            JSONNode requestBody = JSON.Parse("{}"); 
            requestBody["model"] = model; 
            requestBody["messages"] = new JSONArray();

            JSONNode systemInitNode = JSON.Parse("{}");
            systemInitNode["role"] = "system";
            systemInitNode["content"] = SystemInitPrompt;

            requestBody["messages"].Add(systemInitNode);

            JSONNode promptNode = JSON.Parse("{}");
            promptNode["role"] = "user";
            promptNode["content"] = promptSend;

            requestBody["messages"].Add(promptNode);

            requestBody["temperature"] = temperature;
            // requestBody["max_tokens"] = maxTokens;
            requestBody["n"] = n;
            requestBody["stream"] = true;
            requestBody["stop"] = "";
    
            string requestBodyString = requestBody.ToString();

            CoroutineHelper.StartCor(SendRequestQ(requestBodyString , index));
        }

        public void SentRequestWithHistory(List<GPTChatMessage> chatMessages)
        {
            model = models[modelIndex];

            JSONNode requestBody = JSON.Parse("{}"); 
            requestBody["model"] = model; 
            requestBody["messages"] = new JSONArray();  

            foreach (GPTChatMessage chatMessage in chatMessages)
            {
                JSONNode messageNode = JSON.Parse("{}");
                messageNode["role"] = chatMessage.role;
                messageNode["content"] = chatMessage.content;

                requestBody["messages"].Add(messageNode);
            }

            // requestBody["max_tokens"] = maxTokens;

            requestBody["n"] = n;
            requestBody["stream"] = true;

            requestBody["stop"] = "";

            requestBody["temperature"] = temperature;

            string requestBodyString = requestBody.ToString();

            CoroutineHelper.StartCor(SendRequestQ(requestBodyString));
        }
  
        private void CheckRequestProgress()
        { 
            if (!currentRequest.isDone){  
                // yield return new WaitForSeconds(0.05f); 

                string currentContent = currentRequest.downloadHandler.text; 

                List<string> lines = new List<string>(currentContent.Split('\n')); 
                lines = lines.FindAll(s => !string.IsNullOrEmpty(s)); 
 
                if(lines.Count == 0 || lastAmount == lines.Count) return;

                List<string> newLines = new List<string>();
                for(int i = lastAmount; i < lines.Count; i++){
                    newLines.Add(lines[i]);
                }
                lastAmount = lines.Count;

                foreach (var line in newLines){
                    string cleanedLine = line.Trim();
                    cleanedLine = cleanedLine.Replace("\"object\"", "\"objectName\"");
                    cleanedLine = cleanedLine.Replace("data:", "").Trim();

                    if (!string.IsNullOrEmpty(cleanedLine) && cleanedLine != "[DONE]"){ 
                        JSONNode jsonNode = JSON.Parse(cleanedLine);

                        GPTResponseStream oaiResponse = GetOAIObject(jsonNode);

                        string newContent = oaiResponse.choices[0].delta.content;

                        reponseText += newContent;   

                        OnPartResponseReceived?.Invoke(newContent);
                    }

                    // if(cleanedLine == "[DONE]"){
                    //     OnPartResponseReceived?.Invoke("[DONE]");
                    // }
                }
            }else{

                if (currentRequest.result == UnityWebRequest.Result.ConnectionError || (currentRequest.result == UnityWebRequest.Result.ProtocolError)){
                    Debug.Log(currentRequest.error); 
                    status = GPTStatus.Error;
                }else{
                    status = GPTStatus.Success;
                }  

                OnResponseReceived?.Invoke(reponseText, 0);  
#if UNITY_EDITOR
                UnityEditor.EditorApplication.update -= CheckRequestProgress;  
#endif
            } 
        }
        public IEnumerator SendRequestQ(string requestBodyString,  int index = 0)
        {   
            #if UNITY_EDITOR 
                // if(apiKey == "")
                loadSavedConfiguration(); 
            #endif
     
            currentRequest = new UnityWebRequest(apiUrl, "POST");
 
            currentRequest.SetRequestHeader("Authorization", "Bearer " + apiKey); 
            currentRequest.SetRequestHeader("Content-Type", "application/json");
 
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBodyString);
            currentRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            currentRequest.downloadHandler = new DownloadHandlerBuffer();

            status = GPTStatus.WaitingForResponse;
 
            currentRequest.SendWebRequest();
             
             reponseText = ""; 
             lastAmount = 0;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.update += CheckRequestProgress; 
#endif

            yield return null;

        }

        public static void StopGeneration()
        {
            if (currentRequest != null)
            {
                currentRequest.Abort(); 
                status = GPTStatus.Idle;
            }
        }

        private static GPTResponseStream GetOAIObject(JSONNode jsonNode)
        {
            GPTResponseStream oaiResponse = new GPTResponseStream();
            oaiResponse.id = jsonNode["id"];
            oaiResponse.objectName = jsonNode["object"];
            oaiResponse.created = jsonNode["created"];
            oaiResponse.model = jsonNode["model"];

            JSONArray jsonChoices = jsonNode["choices"].AsArray;
            oaiResponse.choices = new GPTResponseStream.Choice[jsonChoices.Count];

            for (int i = 0; i < jsonChoices.Count; i++)
            {
                oaiResponse.choices[i] = new GPTResponseStream.Choice();
                oaiResponse.choices[i].index = jsonChoices[i]["index"];

                JSONNode deltaNode = jsonChoices[i]["delta"];
                oaiResponse.choices[i].delta = new GPTResponseStream.Delta();
                oaiResponse.choices[i].delta.role = deltaNode["role"];
                oaiResponse.choices[i].delta.content = deltaNode["content"];
            }
    
            return oaiResponse;
        }
    #if UNITY_EDITOR 
        private void loadSavedConfiguration(){
            apiKey = UnityEditor.EditorPrefs.GetString("UAISecretKey", "");
            model = UnityEditor.EditorPrefs.GetString("GPTModel", "gpt-3.5-turbo");
            modelIndex = UnityEditor.EditorPrefs.GetInt("GPTModelIndex", 0);
            temperature = UnityEditor.EditorPrefs.GetFloat("GPTTemperature", 0.7f);
            maxTokens = UnityEditor.EditorPrefs.GetInt("GPTMaxTokens", 3000);
            n = 1;//UnityEditor.EditorPrefs.GetInt("GPTN", 1); 
            defaultSavePath = UnityEditor.EditorPrefs.GetString("GPTDefaultSavePath", "Assets/");
            askForSavePath = UnityEditor.EditorPrefs.GetBool("GPTAskForSavePath", false);
        }
    #endif

    }


    public enum GPTStatus {
        Idle,
        WaitingForResponse, 
        Success,
        Error
    }
}