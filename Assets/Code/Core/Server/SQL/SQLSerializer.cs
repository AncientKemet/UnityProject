using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Libaries.IO;
using UnityEngine;
using Object = System.Object;

namespace Server.SQL
{
    public static class SQLSerializer
    {
        #region Optimalizations
        /// <summary>
        /// Holds List of fields to serialize for each class.
        /// </summary>
        private static Dictionary<Type, List<FieldInfo>> _CachedClassSerializations = new Dictionary<Type, List<FieldInfo>>();
        #endregion

        /// <summary>
        /// Serializes an object into binaryform of json.
        /// </summary>
        /// <param name="serializable">Object to be serialized</param>
        public static JSONObject Serialize(ISQLSerializable serializable)
        {
            //create an instance of json
            JSONObject json = new JSONObject("MainObject");

            //run the complex function
            json.AddField(serializable.GetType().ToString(), SerializeIntoJSONObject(serializable));

            //return the result
            return json;
        }

        private static JSONObject SerializeIntoJSONObject(ISQLSerializable serializable)
        {
            if(serializable == null)
                return new JSONObject();

            JSONObject local = new JSONObject();

            //get the type of the object
            Type type = serializable.GetType();

            //is the type already cached?
            bool alreadyCached = _CachedClassSerializations.ContainsKey(type);

            //if not run the cache function
            if (!alreadyCached)
                CacheType(type);

            //get the list of fields to serialize
            List<FieldInfo> fieldInfos = _CachedClassSerializations[type];

            //Serialize each field
            foreach (var field in fieldInfos)
            {
                local.AddField(field.Name, SerializeFieldInfoIntoJSONObject(field, serializable));
            }

            return local;
        }

        #region Field Serialization
        /// <summary>
        /// Serializes a field into json.
        /// </summary>
        /// <param name="fieldinfo">Info</param>
        /// <param name="serializable">Object</param>
        /// <param name="json">OutputBytestrem</param>
        private static JSONObject SerializeFieldInfoIntoJSONObject(FieldInfo fieldinfo, ISQLSerializable serializable)
        {

            JSONObject local = new JSONObject();

            Type type = fieldinfo.FieldType;

            if (typeof(ISQLSerializable).IsAssignableFrom(type))
            {
                ISQLSerializable valueSerializable = fieldinfo.GetValue(serializable) as ISQLSerializable;
                local.AddField(type.ToString(), SerializeIntoJSONObject(valueSerializable));
            }
            else
            {
                Object value = fieldinfo.GetValue(serializable);

                if(IsNumber(value))
                    local.Add(value.ToString());

                if (typeof (IList).IsAssignableFrom(type))
                {
                    IList list = value as IList;
                    local.AddField("Count", list.Count);
                    int i = 0;
                    foreach (var o in list)
                    {
                        local.AddField(i+"", SerializeIntoJSONObject(o as ISQLSerializable));
                        i++;
                    }
                }

            }

            return local;
        }


        public static bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }
        
        #endregion

        /// <summary>
        /// Caches a type, looking for fields with [SQL] attribute.
        /// </summary>
        /// <param name="type">the type to cache</param>
        private static void CacheType(Type type)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                         BindingFlags.Static | BindingFlags.Instance |
                         BindingFlags.DeclaredOnly;
            if (!_CachedClassSerializations.ContainsKey(type))
            {
                List<FieldInfo> _serializedFields = new List<FieldInfo>();
                
                //look for fields inside this class
                {
                    foreach (var field in type.GetFields(flags))
                    {
                        foreach (var customAttribute in field.GetCustomAttributes(typeof(SQLSerialize), true))
                        {
                            if (customAttribute is SQLSerialize)
                            {
                                _serializedFields.Add(field);
                            }
                        }
                    }
                }

                //look for fields in base classes
                {
                    Type baseType = type.BaseType;

                    while (baseType != null && baseType != typeof (Object))
                    {

                        foreach (var field in baseType.GetFields(flags))
                        {
                            foreach (var customAttribute in field.GetCustomAttributes(typeof(SQLSerialize),true))
                            {
                                if (customAttribute is SQLSerialize)
                                {
                                    _serializedFields.Add(field);
                                }
                            }
                        }
                        baseType = baseType.BaseType;
                    }
                }

                _CachedClassSerializations.Add(type, _serializedFields);
            }
        }
    }
}
