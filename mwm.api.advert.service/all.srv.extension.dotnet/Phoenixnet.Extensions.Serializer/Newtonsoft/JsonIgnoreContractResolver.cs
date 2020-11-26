using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Phoenixnet.Extensions.Serializer.Newtonsoft
{
    [Flags]
    public enum SkipOption
    {
        None = 0,
        JsonIgnore = 1,
        JsonProperty = 2
    }

    public class JsonIgnoreContractResolver : DefaultContractResolver
    {
        private readonly SkipOption _skipOption;

        public JsonIgnoreContractResolver()
        {
            _skipOption = SkipOption.None;
        }

        public JsonIgnoreContractResolver(SkipOption skipOption)
        {
            _skipOption = skipOption;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if ((_skipOption & SkipOption.JsonIgnore) == SkipOption.JsonIgnore)
            {
                property.Ignored = false;
            }

            if ((_skipOption & SkipOption.JsonProperty) == SkipOption.JsonProperty)
            {
                property.PropertyName = property.UnderlyingName;
            }

            return property;
        }
    }
}