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
        public DragDropEventData(bool dummyEvent) 
        {
            this.dummy = dummyEvent;
        }

        public static DragDropEventData current = new DragDropEventData(false);

        DragDropItem m_item;
        DragDropTarget m_target;


        public int matchingChannel;
        public bool eligibleForDrag;
        public bool eligibleForClick;
        
        public readonly bool dummy;

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
            matchingChannel = 0;
            eligibleForDrag = false;
            eligibleForClick = false;
            target = null;
            item = null;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\n---");
            sb.AppendFormat(" <b>dummy</b> = {0} \n", dummy);
            sb.AppendFormat(" <b>item</b> = {0} \n", item);
            sb.AppendFormat(" <b>target</b> = {0} \n", target);
            sb.AppendFormat(" <b>matchingChannel</b> = B{0} \n", Convert.ToString(matchingChannel, 2));
            sb.AppendFormat(" <b>eligibleForDrag</b> = {0} \n", eligibleForDrag);
            sb.AppendFormat(" <b>eligibleForClick</b> = {0} \n", eligibleForClick);
            sb.Append("---");
            return sb.ToString();
        }

    }

}
