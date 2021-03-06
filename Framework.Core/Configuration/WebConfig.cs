﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Framework.Core.Configuration
{
    public class WebConfig : IConfigurationSectionHandler
    {

        public object Create(object parent, object configContext, XmlNode section)
        {
            return Create(section);
        }

        public WebConfig Create(XmlNode section)
        {
            var config = new WebConfig();

            //StartUp
            var startupNode = section.SelectSingleNode("IgnoreStartupTasks");
            if (startupNode?.Attributes != null)
            {
                var attribute = GetBool(startupNode, "Enabled");
                config.IgnoreStartupTasks = attribute;
            }

            //Redis
            var redisNode = section.SelectSingleNode("RedisCaching");
            if (redisNode?.Attributes != null)
            {
                //Reids Enabled
                var redisEnabled = GetBool(startupNode, "Enabled");
                config.RedisCachingEnabled = redisEnabled;
                //Redis Connection String
                var redisConnection = GetString(startupNode, "ConnectionString");
                if (redisConnection != null)
                    config.RedisCachingConnectionString = redisConnection;
            }

            return config;
        }

        #region Util

        private string GetString(XmlNode node, string attrName)
        {
            return SetByXElement<string>(node, attrName, Convert.ToString);
        }

        private bool GetBool(XmlNode node, string attrName)
        {
            return SetByXElement<bool>(node, attrName, Convert.ToBoolean);
        }

        private T SetByXElement<T>(XmlNode node, string attrName, Func<string, T> converter)
        {
            if (node == null || node.Attributes == null) return default(T);
            var attr = node.Attributes[attrName];
            if (attr == null) return default(T);
            var attrVal = attr.Value;
            return converter(attrVal);
        }

        #endregion

        #region Prop

        /// <summary>
        /// 是否运行应用程序启动任务
        /// </summary>
        public bool IgnoreStartupTasks { get; set; }

        /// <summary>
        /// 指示是否应该使用Redis服务器进行缓存（而不是默认的内存中缓存）
        /// </summary>
        public bool RedisCachingEnabled { get; private set; }

        /// <summary>
        /// Redis连接字符串。 在启用Redis缓存时使用
        /// </summary>
        public string RedisCachingConnectionString { get; private set; }

        #endregion

    }
}
