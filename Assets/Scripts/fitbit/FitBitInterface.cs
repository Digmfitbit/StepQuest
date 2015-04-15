
using OAuth;
using UnityEngine;

public interface IFitBitInterface {

    int getStepsSinceLastCall();
}

public class FitBitHelper : IFitBitInterface
{
    private string consumerKey = "e80b47146ecb43c2b91767db41509a07";
    private string consumerSecret = "427258ee3dd04ee78f53376ee209d26a";
    private string requestTokenURL = "https://api.fitbit.com/oauth/request_token";
    private string AccessTokenURL = "https://api.fitbit.com/oauth/access_token";
    private string authorizeURL = "https://www.fitbit.com/oauth/authorize";

    private static FitBitHelper instance = null;

    private FitBitHelper()
    {
        Manager oauth = new Manager();
        // the URL to obtain a temporary "request token"
        string rtUrl = "https://api.fitbit.com/oauth/request_token";
        oauth["consumer_key"] = consumerKey;
        oauth["consumer_secret"] = consumerSecret;
        //oauth.AcquireRequestToken(rtUrl, "POST");
        Debug.Log("oauth token: "+oauth["token"]);
        //NEED TO GET USER CREDENTIALS SENT

        //string url = authorizeURL + oauth["token"];
        //Application.OpenURL(url);
    }

    public static FitBitHelper getInstance()
    {
        if (instance == null)
        {
            instance = new FitBitHelper();
        }
        return instance;
    }

    public int getStepsSinceLastCall()
    {
        int steps = 0;


        return steps;
    }
}
