﻿using System;
using UnityEngine;
using Utils.Disposables;

namespace Utils
{
    public static class UnityUtils
    {
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            var json = JsonUtility.ToJson(source);
            return JsonUtility.FromJson<T>(json);
        }

        public static Sprite LoadEmptySprite()
        {
            var sprite = Resources.Load<Sprite>("ResourcesSprites/EmptySprite");
            return sprite;
        }

        public static IDisposable Subscribe(this Action action, Action subscribeAction)
        {
            var a = action;
            a += subscribeAction;
            return new ActionDisposable(() =>  a -= subscribeAction);
        } 
    }
}