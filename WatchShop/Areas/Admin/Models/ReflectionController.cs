using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace WatchShop.Areas.Admin.Models
{
    public class ReflectionController
    {
        // Get all controllers in specified area (namespace)
        public static List<Type> GetControllers(string namespaces)
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            IEnumerable<Type> types = asm.GetTypes().Where(type => typeof(Controller).
                IsAssignableFrom(type) && type.Namespace.Contains(namespaces)).OrderBy(x => x.Name);
            return types.ToList();
        }

        // Get all actions by controller
        public static List<string> GetActions(Type controller)
        {
            List<string> actions = new List<string>();
            IEnumerable<MemberInfo> mems = controller.GetMethods(
                BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public).
                Where(m => !m.GetCustomAttributes(
                    typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any()).OrderBy(x => x.Name);
            foreach (MemberInfo method in mems)
            {
                if (method.ReflectedType.IsPublic && !method.IsDefined(typeof(NonActionAttribute)))
                {
                    actions.Add(method.Name.ToString());
                }
            }
            int size = actions.Count;
            for (int i = 0; i < size - 1; i++)
            {
                if (actions[i].Equals(actions[i + 1]))
                {
                    actions.Remove(actions[i + 1]);
                    size--;
                }
            }
            return actions;
        }
    }
}