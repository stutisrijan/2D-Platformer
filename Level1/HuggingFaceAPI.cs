using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HuggingFaceAPI : MonoBehaviour
{
    private string apiUrl = "https://api-inference.huggingface.co/models/gpt2";
    private string apiToken = "huggingfacekey";

    void Start()
    {
        // Start generating jokes every 10 seconds
        StartCoroutine(GenerateJokes());
    }

    private IEnumerator GenerateJokes()
    {
        while (true) // Infinite loop to generate jokes
        {
            string jokePrompt = $"Tell me a funny joke. (Time: {System.DateTime.Now.ToString("HH:mm:ss")})"; // Add time to prompt
            Debug.Log("Sending Prompt: " + jokePrompt);

            yield return StartCoroutine(CallHuggingFaceAPI(jokePrompt));

            // Wait for 10 seconds before generating the next joke
            yield return new WaitForSeconds(10f);
        }
    }

    private IEnumerator CallHuggingFaceAPI(string prompt)
    {
        // Define the payload structure
        var payload = new HuggingFacePayload
        {
            inputs = prompt,
            parameters = new Parameters
            {
                max_length = 50, // Adjust length to suit joke format
                temperature = 1.0f // Increase temperature for more randomness
            }
        };

        string jsonPayload = JsonUtility.ToJson(payload);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Add authorization and content type headers
        request.SetRequestHeader("Authorization", "Bearer " + apiToken);
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);

            // Deserialize the response
            string jsonResponse = request.downloadHandler.text;

            // Parse the first joke from the response array
            string joke = ExtractTextFromArray(jsonResponse);
            Debug.Log("Generated Joke: " + joke);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    private string ExtractTextFromArray(string jsonResponse)
    {
        // Use Unity's JsonUtility to parse the first element of the array
        try
        {
            // Remove the surrounding brackets to treat it as a single object
            string cleanJson = jsonResponse.TrimStart('[').TrimEnd(']');
            HuggingFaceResponse response = JsonUtility.FromJson<HuggingFaceResponse>(cleanJson);
            return response.generated_text;
        }
        catch
        {
            Debug.LogError("Unable to parse response. Response might not match expected format: " + jsonResponse);
            return "Error in joke generation!";
        }
    }

   
    [System.Serializable]
    public class Parameters
    {
        public int max_length;
        public float temperature;
    }

    [System.Serializable]
    public class HuggingFacePayload
    {
        public string inputs;
        public Parameters parameters;
    }

   
    [System.Serializable]
    public class HuggingFaceResponse
    {
        public string generated_text;
    }
}