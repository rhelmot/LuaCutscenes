﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celeste;
using Monocle;
using System.Reflection;
using Microsoft.Xna.Framework;
using Celeste.Mod.Helpers;
using NLua;
using System.Collections;

namespace Celeste.Mod.LuaCutscenes
{
    static class MethodWrappers
    {
        private static MethodInfo getEntityMethodInfo = Engine.Scene.Tracker.GetType().GetMethod("GetEntity");
        private static MethodInfo getEntitiesMethodInfo = Engine.Scene.Tracker.GetType().GetMethod("GetEntities");
        private static MethodInfo entitiesFindFirstMethodInfo = Engine.Scene.Entities.GetType().GetMethod("FindFirst");
        private static MethodInfo entitiesFindAll = Engine.Scene.Entities.GetType().GetMethod("FindAll");

        public static Type GetTypeFromString(string name)
        {
            return FakeAssembly.GetFakeEntryAssembly().GetType("Celeste." + name);
        }

        public static object GetEntity(string name)
        {
            return GetEntity(GetTypeFromString(name));
        }

        public static object GetEntity(Type type)
        {
            try
            { 
                return getEntityMethodInfo.MakeGenericMethod(new Type[] { type }).Invoke(Engine.Scene.Tracker, null);
            }
            catch (ArgumentNullException)
            {
                Logger.Log(LogLevel.Error, "Lua Cutscenes", $"Failed to get entity: Requested type does not exist");
            }
            catch (TargetInvocationException)
            {
                Logger.Log(LogLevel.Error, "Lua Cutscenes", $"Failed to get entity: '{type}' is not trackable");
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, "Lua Cutscenes", $"Failed to get entity: {e}");
            }

            return null;
        }

        public static LuaTable GetEntities(string name)
        {
            return GetEntities(GetTypeFromString(name));
        }

        public static LuaTable GetEntities(Type type)
        {
            try
            {
                return LuaHelper.ListToLuaTable(getEntitiesMethodInfo.MakeGenericMethod(new Type[] { type }).Invoke(Engine.Scene.Tracker, null) as IList);
            }
            catch (ArgumentNullException)
            {
                Logger.Log(LogLevel.Error, "Lua Cutscenes", $"Failed to get entities: Requested type does not exist");
            }
            catch (TargetInvocationException)
            {
                Logger.Log(LogLevel.Error, "Lua Cutscenes", $"Failed to get entities: '{type}' is not trackable");
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, "Lua Cutscenes", $"Failed to get entities: {e}");
            }

            return null;
        }

        // Do not confuse with GetEntity, this also works on non Tracked entities
        public static object GetFirstEntity(string name)
        {
            return GetFirstEntity(GetTypeFromString(name));
        }

        // Do not confuse with GetEntity, this also works on non Tracked entities
        public static object GetFirstEntity(Type type)
        {
            try
            {
                return entitiesFindFirstMethodInfo.MakeGenericMethod(new Type[] { type }).Invoke(Engine.Scene.Entities, null);
            }
            catch (ArgumentNullException)
            {
                Logger.Log(LogLevel.Error, "Lua Cutscenes", $"Failed to get entities: Requested type does not exist");
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, "Lua Cutscenes", $"Failed to get entity: {e}");
            }

            return null;
        }

        // Do not confuse with GetEntity, this also works on non Tracked entities
        public static object GetAllEntities(string name)
        {
            return GetAllEntities(GetTypeFromString(name));
        }

        // Do not confuse with GetEntities, this also works on non Tracked entities
        public static object GetAllEntities(Type type)
        {
            try
            {
                return LuaHelper.ListToLuaTable(entitiesFindAll.MakeGenericMethod(new Type[] { type }).Invoke(Engine.Scene.Entities, null) as IList);
            }
            catch (ArgumentNullException)
            {
                Logger.Log(LogLevel.Error, "Lua Cutscenes", $"Failed to get entities: Requested type does not exist");
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, "Lua Cutscenes", $"Failed to get entity: {e}");
            }

            return null;
        }
    }
}