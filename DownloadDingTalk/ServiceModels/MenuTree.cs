namespace DownloadDingTalk.ServiceModels
{
    class MenuTree
    {
        /// <summary>
        /// 目录Id，对应钉钉的data-dentryId
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 目录Id，对应钉钉的data-dentryUuid
        /// 用于拼接Url访问文档或者目录
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// 节点类型,folder或file，对应data-dentryType
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// 目录名称，对应钉钉的data-name
        /// </summary>
        public string Name { get; set; }

        public IEnumerable<MenuTree> Subtree { get; set; }
    }
}
