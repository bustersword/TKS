using System.Collections.Generic;

namespace TKS.Common
{
    public class MenuEntity
    {
        /// <summary>
        /// 菜单名
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单图片路径
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 菜单url
        /// </summary>
        public string MenuUrl { get; set; }

        /// <summary>
        /// 详细描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 二级菜单
        /// </summary>
        public List<MenuEntity> ChildMenu { get; set; }
    }
}
