using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class LoginPage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

  IEnumerator LoginFun(string url)
    {
        string data = "";
        UnityWebRequest request = UnityWebRequest.Post(url, data);
        yield return request.SendWebRequest();
        if(request.isNetworkError||request.isHttpError)
        {
            Debug.Log("wangluobutong");
        }
        else
        {
            User user;
            user = JsonUtility.FromJson<User>(request.downloadHandler.ToString());
            if(user.code==0)
            {

            }
            else
            {

            }
        }

    }

    void LoginPost(string url)
    {

    }
    IEnumerator LoginPostIe(string url, string name, string pwd)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(name);
        sb.Append(':');
        sb.Append(pwd);
        UnityWebRequest request = UnityWebRequest.Post(url, sb.ToString());
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {

        }
        else
        {
            User user;
            user = JsonUtility.FromJson<User>(request.downloadHandler.ToString());
            if (user.code == 0)
            {

            }
        }
    }

    public async void LoginAsync(string name,string pwd)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(name);
        sb.Append(':');
        sb.Append(pwd);
        var request = UnityWebRequest.Post("",sb.ToString());
        await request.SendWebRequest();


    }



}


public static class ExtensionMethods
{
    public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOperation)
    {
        var tcs = new TaskCompletionSource<object>();
        asyncOperation.completed += obj => { tcs.SetResult(null); };
        return ((Task)tcs.Task).GetAwaiter();
    }
}

class User
{
    public int code;
    string name;
        string pwd;
}
