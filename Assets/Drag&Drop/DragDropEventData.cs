namespace AillieoUtils
{

    public enum DragDropEventTriggerType
    {

        // for both item & target
        ItemExit = 1,
        ItemEnter = 2,
        ItemDetach = 3,
        ItemAttach = 4,

        // for target only
        ItemSetFree = 5,
        ItemClick = 6,

    }


    public class DragDropEventData
    {
        public static DragDropEventData current = new DragDropEventData();

        DragDropItem m_item;
        DragDropTarget m_target;


        public int matchingChannel;
        public bool eligibleForDrag;
        public bool eligibleForClick;

        public bool isReplaced;

        public bool external;

        public bool valid
        {
            get
            {
                return (m_target!=null && m_item != null && DragDropHelper.IsChannelMatch(m_target,m_item));
            }
        }


        public DragDropItem item
        {
            get
            {
                return m_item;
            }
            set
            {
                if (m_item != value)
                {
                    m_item = value;
                    if (m_item)
                    {
                        matchingChannel = m_item.matchingChannel;
                    }
                }
            }
        }


        public DragDropTarget target
        {
            get
            {
                return m_target;
            }
            set
            {
                if (m_target != value)
                {
                    m_target = value;
                }
            }
        }


        public void Reset()
        {
            external = false;
            matchingChannel = 0;
            eligibleForDrag = false;
            eligibleForClick = false;
            isReplaced = false;
            target = null;
            item = null;
        }


        public override string ToString()
        {
#if UNITY_EDITOR
            return string.Format("\n---\n   <b>matchingChannel</b> = {0} \n   <b>item</b> = {1} \n   <b>target</b> = {2}\n---", matchingChannel, item,target);
#else
            return "";
#endif
        }

    }

}