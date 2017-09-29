using System;

namespace PayMe.Apps.ViewModels
{

    public class PrincipalMainPageMenuItem
    {

        public PrincipalMainPageMenuItem(Type type)
        {
            TargetType = type;
        }

        public PrincipalMainPageMenuItem(Type type, params object[] parameters)
        {
            TargetType = type;
            Parameters = parameters;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public object[] Parameters { get; set; }
        public Type TargetType { get; set; }
    }
}