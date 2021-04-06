using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace 框架搭建.Filter
{
    /// <summary>
    /// 自定来定义了一个特性 来完成支持其他Filter的依赖注入----使用容器来做对象的实例化；
    /// </summary>
    public class CustomIOCFilterFactoryAttribute : Attribute, IFilterFactory
    {
        private Type _Type = null;

        public CustomIOCFilterFactoryAttribute(Type type)
        {
            _Type = type;
        }

        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            object oInstance = serviceProvider.GetService(_Type);
            return (IFilterMetadata)oInstance;
        }
    }
}
