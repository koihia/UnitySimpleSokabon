using System;
using System.Collections.Generic;
using System.Linq;
using Sokabon.CommandSystem;
using UnityEngine;

namespace Sokabon
{
    public class BlockCollisionDetector : MonoBehaviour
    {
        [SerializeField] private LayerSettings layerSettings;

        private Block _block;
        private TurnManager _turnManager;

        private void Awake()
        {
            _block = GetComponent<Block>();

            _turnManager = FindObjectOfType<TurnManager>();
        }

        private void OnEnable()
        {
            _block.AtNewPositionEvent += CheckForCollision;
        }

        private void OnDisable()
        {
            _block.AtNewPositionEvent -= CheckForCollision;
        }

        private void CheckForCollision(bool isReplay)
        {
            if (isReplay)
            {
                return;
            }

            var cols = Physics2D.OverlapCircleAll(_block.rb.position, 0.3f, layerSettings.blockLayerMask);
            var blocks = cols.Select(col => col.GetComponent<Block>()).Where(block => block is not null).ToList();
            if (blocks.Count() < 2)
            {
                return;
            }

            foreach (var block in blocks)
            {
                _turnManager.ExecuteCommand(new BlockDisable(block));
            }
        }
    }
}