namespace Qihoo.CloudEngine
{
    /// <summary>
    /// 扫描结果
    /// </summary>
    public class FileHealthResult
    {
        /// <summary>
        /// 威胁类型
        /// </summary>
        public string? MalwareType { get; private set; } = "Unknow Type";
        /// <summary>
        /// 是否为已知威胁
        /// </summary>
        public bool IsVerifiedMalware { get; private set; } = false;
        /// <summary>
        /// 是否上传过
        /// </summary>
        public bool HasUpload { get; private set; } = false;
        /// <summary>
        /// 第一次发现以来经过的天数
        /// </summary>
        public int Age { get; private set; } = 0;
        /// <summary>
        /// 流行度
        /// </summary>
        public int Pop { get; private set; } = 0;
        /// <summary>
        /// 威胁研判等级 0-100
        /// </summary>
        public float Level { get; private set; } = 0;
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsOperationSuccess { get; private set; } = false;

        internal FileHealthResult(string type, bool known, bool upload, int age, int pop, float elevel, bool issuccess)
        {
            MalwareType = type;
            IsVerifiedMalware = known;
            HasUpload = upload;
            Age = age;
            Pop = pop;
            Level = elevel;
            IsOperationSuccess = issuccess;
        }
    }
}