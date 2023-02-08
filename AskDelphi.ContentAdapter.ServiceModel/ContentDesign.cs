using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.ServiceModel
{
    public class ContentDesignDetails : object
    {

        public CddPyramidLevel[] PyramidLevels { get; set; }

        public string Title { get; set; }

        public CddTopicTypeGroup[] TopicTypeGroups { get; set; }

        public CddTopicType[] TopicTypes { get; set; }
    }

    public class CddTopicType : object
    {

        public string DisplayName { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsFromContentDesign { get; set; }

        public System.Guid Key { get; set; }

        public string Namespace { get; set; }

        public CddTopicTypeRelation[] Relations { get; set; }

        public string Title { get; set; }
    }

    public class CddTopicTypeGroup : object
    {

        public string DisplayName { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsFromContentDesign { get; set; }

        public bool IsTopicListFilter { get; set; }

        public System.Guid Key { get; set; }

        public string Role { get; set; }

        public string Title { get; set; }

        public CddTopicType[] TopicTypes { get; set; }
    }

    public class CddTopicTypeRelation : object
    {

        public string DisplayName { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsFromContentDesign { get; set; }

        public bool IsInternal { get; set; }

        public System.Guid Key { get; set; }

        public CddPyramidLevel PyramidLevel { get; set; }

        public string Title { get; set; }

        public System.Guid TopicTypeGroupKey { get; set; }

        public string Use { get; set; }

        public string View { get; set; }
    }

    public class CddPyramidLevel : object
    {

        public string DisplayName { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsFromContentDesign { get; set; }

        public System.Guid Key { get; set; }

        public int SequenceNumber { get; set; }

        public string Title { get; set; }
    }
    }
