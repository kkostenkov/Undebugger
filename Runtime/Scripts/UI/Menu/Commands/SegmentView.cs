using Deszz.Undebugger.Model.Commands;
using Deszz.Undebugger.UI.Layout;
using UnityEngine;
using UnityEngine.UI;

namespace Deszz.Undebugger.UI.Menu.Commands
{
    public class SegmentView : MonoBehaviour
    {
        [SerializeField]
        private Text nameText;
        [SerializeField]
        private RectTransform container;
        [SerializeField]
        private FlexibleGrid layout;

        private CommandView[] commands;

        public void Init(SegmentModel model, CommandViewFactory optionViewFactory)
        {
            Deinit();

            nameText.text = model.Name == null ? "Unnamed" : model.Name;
            commands = new CommandView[model.Commands.Count];

            for (int i = 0; i < model.Commands.Count; ++i)
            {
                var template = optionViewFactory.FindTemplate(model.Commands[i].GetType());

                commands[i] = Instantiate(template, container);
                commands[i].Setup(model.Commands[i]);
            }
        }

        private void Deinit()
        {
            if (commands != null)
            {
                for (int i = 0; i < commands.Length; ++i)
                {
                    Destroy(commands[i].gameObject);
                }

                commands = null;
            }
        }
    }
}
