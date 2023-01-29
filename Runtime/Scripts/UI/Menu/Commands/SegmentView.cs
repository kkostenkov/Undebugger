using System.Collections.Generic;
using Undebugger.Model.Commands;
using Undebugger.UI.Layout;
using UnityEngine;
using UnityEngine.UI;

namespace Undebugger.UI.Menu.Commands
{
    public class SegmentView : MonoBehaviour, IPoolable, IPoolHandler
    {
        [SerializeField]
        private Text nameText;
        [SerializeField]
        private RectTransform container;
        [SerializeField]
        private FlexibleGrid layout;

        private List<CommandView> commands;
        private MenuPool pool;

        public void UsePool(MenuPool pool)
        {
            this.pool = pool;
        }

        public void AddingToPool()
        {
            Deinit();
        }

        public void Init(SegmentModel model, CommandViewFactory optionViewFactory)
        {
            Deinit();

            if (commands == null)
            {
                commands = new List<CommandView>(capacity: model.Commands.Count);
            }

            nameText.text = model.Name == null ? "Unnamed" : model.Name;

            for (int i = 0; i < model.Commands.Count; ++i)
            {
                var template = optionViewFactory.FindTemplate(model.Commands[i].GetType());

                if (pool == null || !pool.TryGet(template.GetType(), out CommandView commandView, container))
                {
                    commandView = Instantiate(template, container);
                }

                commandView.Setup(model.Commands[i]);
                commands.Add(commandView);
            }
        }

        public void Deinit()
        {
            if (commands != null)
            {
                for (int i = 0; i < commands.Count; ++i)
                {
                    if (pool != null)
                    {
                        pool.Add(commands[i]);
                    }
                    else
                    {
                        DestroyImmediate(commands[i].gameObject);
                    }
                }

                commands.Clear();
            }
        }
    }
}
