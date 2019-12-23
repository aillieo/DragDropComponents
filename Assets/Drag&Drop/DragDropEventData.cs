using System;
using System.Text;

namespace AillieoUtils
{

    public enum DragDropEventTriggerType
    {

        // for both item & target
        ItemExit = 1,
        ItemEnter = 2,
        ItemDetach = 3,
        ItemAttach = 4,

        // for item only
        ItemSetFree = 5,
        ItemClick = 6,
        ItemDrag = 7,

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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\n---");
            sb.AppendFormat(" <b>item</b> = {0} \n", item);
            sb.AppendFormat(" <b>target</b> = {0} \n", target);
            sb.AppendFormat(" <b>matchingChannel</b> = B{0} \n", Convert.ToString(matchingChannel, 2));
            sb.AppendFormat(" <b>external</b> = {0} \n", external);
            sb.AppendFormat(" <b>eligibleForDrag</b> = {0} \n", eligibleForDrag);
            sb.AppendFormat(" <b>eligibleForClick</b> = {0} \n", eligibleForClick);
            sb.AppendFormat(" <b>isReplaced</b> = {0} \n", isReplaced);
            sb.Append("---");
            return sb.ToString();
#else
            return "";
#endif
        }

    }

}