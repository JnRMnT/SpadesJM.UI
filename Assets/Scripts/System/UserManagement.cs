using UnityEngine;
using System.Collections;

using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.user;

using AssemblyCSharp;
using System;

public class UserManagement : MonoBehaviour
{
    public MultiplayerManager main_mpserver;
    public UserService userService;
    public RegisterResultScreen reg_result_screen;

    public void InitializeUserManagement()
    {
        userService = main_mpserver.ServiceApi.BuildUserService();
        App42Log.SetDebug(LogManager.IsActive);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void doRegister(string email, string username, string pw)
    {
        CreateUserCallback createUserCallback = new CreateUserCallback();
        try
        {
            userService.CreateUser(username, pw, email, createUserCallback);
        }
        catch (Exception e)
        {
            createUserCallback.OnException(e);
        }
    }

    public void doLogin(string username, string pw)
    {
        userService.Authenticate(username, pw,new LoginCallBack());
    }

    public class LoginCallBack : App42CallBack
    {
        public void OnSuccess(object response)
        {
            User user = (User)response;
            App42Log.Console("userName is " + user.GetUserName());
            App42Log.Console("sessionId is " + user.GetSessionId());
            MultiplayerManager.connectToWarp(user.GetUserName());
            PlayerPrefs.SetString("username", user.GetUserName());
            LoginResultScreen.login_result = 0;
        }
        public void OnException(Exception e)
        {
            App42Log.Console("Exception : " + e);
            int appErrorCode = ((App42Exception)e).GetAppErrorCode();

            if (e.Message.Contains("check your network"))
            {
                LoginResultScreen.login_result = 404;
            }
            else if (appErrorCode == 0)
            {
                LoginResultScreen.login_result = 99;
            }
            else
            {
                LoginResultScreen.login_result = appErrorCode;
            }

        }
    }

    public class CreateUserCallback : App42CallBack
    {
        public void OnSuccess(object response)
        {
            User user = (User)response;
            App42Log.Console("userName is " + user.GetUserName());
            App42Log.Console("emailId is " + user.GetEmail());
            MultiplayerManager.connectToWarp(user.GetUserName());
            PlayerPrefs.SetString("username", user.GetUserName());
            //	REGISTER SUCCESSFUL
            RegisterResultScreen.setRegResult(0);
        }
        public void OnException(Exception e)
        {
            if (e.ToString().Contains("already exists"))
            {
                LogManager.Log("User already exists");
                RegisterResultScreen.setRegResult(1);
            }
            else if (e.Message.Contains("EmailAddress is Not Valid"))
            {
                LogManager.Log("EmailAddress is Not Valid");
                RegisterResultScreen.setRegResult(2);
            }
            else App42Log.Console("Exception : " + e);
        }
    }
}
