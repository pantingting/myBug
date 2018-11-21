﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Config 的摘要说明
/// </summary>
public static class Config
{
    private static bool noCache = true;
    private static JObject BuildItems()
    {
        var json = File.ReadAllText(HttpContext.Current.Server.MapPath("config.json"));
        json = json.Replace("$", GetBaseURL(HttpContext.Current.Request));
        return JObject.Parse(json);
    }

    public static string GetBaseURL(HttpRequest request)
    {
        if (request.IsSecureConnection)
        {
            return @"https://" + request.Url.Authority + request.ApplicationPath;
        }
        else
        {
            return @"http://" + request.Url.Authority + request.ApplicationPath;
        }
    }

    public static JObject Items
    {
        get
        {
            if (noCache || _Items == null)
            {
                _Items = BuildItems();
            }
            return _Items;
        }
    }
    private static JObject _Items;


    public static T GetValue<T>(string key)
    {
        return Items[key].Value<T>();
    }

    public static String[] GetStringList(string key)
    {
        return Items[key].Select(x => x.Value<String>()).ToArray();
    }

    public static String GetString(string key)
    {
        return GetValue<String>(key);
    }

    public static int GetInt(string key)
    {
        return GetValue<int>(key);
    }
}