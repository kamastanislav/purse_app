﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PurseApi.Models.Helpers
{
    public class FastInvoke
    {
        public static Action<T, object> BuildUntypedSetter<T>(PropertyInfo propertyInfo)
        {
            var targetType = propertyInfo.DeclaringType;
            var methodInfo = propertyInfo.SetMethod;
            var exTarget = Expression.Parameter(targetType, "t");
            var exValue = Expression.Parameter(typeof(object), "p");
            var exBody = Expression.Call(exTarget, methodInfo,
            Expression.Convert(exValue, propertyInfo.PropertyType));
            var lambda = Expression.Lambda < Action < T, object>>(exBody, exTarget, exValue);
            var action = lambda.Compile();
            return action;
        }

        /*
        public static Func<T, TReturn> BuildTypedGetter<T, TReturn>(PropertyInfo propertyInfo)
        {
            Func<T, TReturn> reflGet = (Func<T, TReturn>)
            Delegate.CreateDelegate(typeof(Func<T, TReturn>), propertyInfo.GetGetMethod());
            return reflGet;
        }

        public static Action<T, TProperty> BuildTypedSetter<T, TProperty>(PropertyInfo propertyInfo)
        {
            Action<T, TProperty> reflSet = (Action<T, TProperty>)Delegate.CreateDelegate(
            typeof(Action<T, TProperty>), propertyInfo.GetSetMethod());
            return reflSet;
        }

        public static Func<T, object> BuildUntypedGetter<T>(PropertyInfo propertyInfo)
        {
            var targetType = propertyInfo.DeclaringType;
            var methodInfo = propertyInfo.GetGetMethod();
            var returnType = methodInfo.ReturnType;

            var exTarget = Expression.Parameter(targetType, "t");
            var exBody = Expression.Call(exTarget, methodInfo);
            var exBody2 = Expression.Convert(exBody, typeof(object));

            var lambda = Expression.Lambda < Func < T, object>>(exBody2, exTarget);
            var action = lambda.Compile();
            return action;
        }*/
    }
}