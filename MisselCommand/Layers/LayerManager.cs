﻿using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MisselCommand.Layers
{
    public class LayerManager
    {
        private LinkedList<ILayer> _layers = new LinkedList<ILayer>();
        private ContentManager _content;

        public LayerManager(ContentManager content)
        {
            _content = content;
        }

        public void addLayer(ILayer layer)
        {
            ILayer llayer = (ILayer)layer;
            llayer.load(_content);
            lock (_layers)
            {
                _layers.AddLast(llayer);
            }
        }

        public void popLayer()
        {
            lock (_layers)
            {
                _layers.RemoveLast();
            }
        }

        public ILayer getLayerById(String id)
        {
            lock (_layers)
            {
                LinkedListNode<ILayer> llayer = _layers.Last;
                while (llayer != null)
                {
                    if (llayer.Value.layerId() == id)
                        return llayer.Value;
                    llayer = llayer.Previous;
                }
            }
            return null;
        }

        public void update()
        {
            lock (_layers)
            {
                LinkedListNode<ILayer> llayer = _layers.Last;
                while (llayer != null)
                {
                    llayer.Value.update(this);
                    llayer = llayer.Previous;
                }
            }
        }

        public void draw()
        {
            lock (_layers)
            {
                LinkedListNode<ILayer> llayer = _layers.Last;
                while (llayer != null)
                {
                    llayer.Value.draw();
                    llayer = llayer.Previous;
                }
            }
        }

    }
}
