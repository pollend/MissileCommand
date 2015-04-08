using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace missileCommand.Layers
{
    public class LayerManager
    {
        private MouseState _mouse = new MouseState();
        private KeyboardState _keyboardState = new KeyboardState();
        private LinkedList<ILayer> _layers = new LinkedList<ILayer>();
        private ContentManager _content;

        public MouseState mouse { get { return _mouse; } }
        public KeyboardState keyboard { get { return _keyboardState; } }

        public LayerManager(ContentManager content)
        {
            _content = content;
        }

        public void addLayer(ILayer layer)
        {
            ILayer llayer = (ILayer)layer;
            llayer.load(_content);
            _layers.AddLast(llayer);
        }

        public void clearLayers()
        {
            _layers.Clear();
        }

        public void popLayer()
        {
           _layers.RemoveLast();
        }

        public ILayer getLayerById(String id)
        {
            LinkedListNode<ILayer> llayer = _layers.Last;
            while (llayer != null)
            {
                if (llayer.Value.layerId() == id)
                    return llayer.Value;
                llayer = llayer.Previous;
            }
            return null;
        }

        public void update()
        {
            _mouse = Mouse.GetState();
            _keyboardState = Keyboard.GetState();
  
            LinkedListNode<ILayer> llayer = _layers.Last;
            while (llayer != null)
            {
                llayer.Value.update(this);
                llayer = llayer.Previous;
            }



        }

        public void draw()
        {
            LinkedListNode<ILayer> llayer = _layers.First;
            while (llayer != null)
            {
                llayer.Value.draw();
                llayer = llayer.Next;
            }
            
        }

    }
}
