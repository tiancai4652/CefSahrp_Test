using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Cef_Course_Test.Base
{
    [DataContract]//元数据定义特性
    public class jsInvokeData//元结构体
    {
        [DataMember]
        public string type { get; set; }//元数据
        [DataMember]
        public object data { get; set; }//元数据
    }

    [DataContract]//元数据定义特性
    public class jsonData<T>//元结构体
    {
        [DataMember]
        public string type { get; set; }//元数据
        [DataMember]
        public T data { get; set; }//元数据
    }

    [DataContract]
    public class currentPageIndexChange
    {
        [DataMember]
        public int currentPageIndex { get; set; }
    }

    //[DataContract]
    //public class PageIndexC
    //{
    //    [DataMember]
    //    public int pageIndex { get; set; }
    //}

    [DataContract]
    public class SlideInfoC
    {
        [DataMember]
        public int totalPageNum { get; set; }

        [DataMember]
        public List<PageInfoC> pageInfo { get; set; }
    }

    [DataContract]
    public class PageInfoC
    {
        [DataMember]
        public int index { get; set; }

        [DataMember]
        public int width { get; set; }

        [DataMember]
        public int height { get; set; }

        [DataMember]
        public string pageName { get; set; }

        [DataMember]
        public string note { get; set; }

        [DataMember]
        public string pageItemType { get; set; }

        [DataMember]
        public int animationSize { get; set; }

        [DataMember]
        public string answer { get; set; }//这是个啥？

        [DataMember]
        public string h5FolderName { get; set; }
    }

    [DataContract]
    public class isLastAnimationC
    {
        [DataMember]
        public bool isLastAniamtion { get; set; }

        [DataMember]
        public bool isFirstAniamtion { get; set; }
    }

    [DataContract]
    public class callerIdC
    {
        [DataMember]
        public int callerId { get; set; }
    }

    [DataContract]
    public class WebPositionC
    {
        [DataMember]
        public int x { get; set; }

        [DataMember]
        public int y { get; set; }
    }

    [DataContract]
    public class H5PageAnswerC
    {
        [DataMember]
        public int index { get; set; }

        [DataMember]
        public string answer { get; set; }
    }

    [DataContract]
    public class FileUrlC
    {
        [DataMember]
        public string fileName { get; set; }

        [DataMember]
        public string remoteUrl { get; set; }
    }

    [DataContract]
    public class slideInfoC
    {
        [DataMember]
        public List<FileUrlC> normalFileList { get; set; }

        [DataMember]
        public List<FileUrlC> zipFileList { get; set; }

        [DataMember]
        public string slideJsonFile { get; set; }
    }
}
