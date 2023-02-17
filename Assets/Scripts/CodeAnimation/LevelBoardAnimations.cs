using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Widgets;
using Random = UnityEngine.Random;

namespace CodeAnimation
{
    [Serializable]
    public class LevelBoardAnimations
    {
        [SerializeField] private int _outLevelOffset = 250;
        
        private Vector2[,] _boardPositions;
        private Vector2[,] _randomPositions;

        public IEnumerator Appearance(List<List<BoardItemWidget>> board)
        {
            _randomPositions ??= new Vector2[board.Count, board[0].Count];
            _boardPositions ??= new Vector2[board.Count, board[0].Count];

            FullRandomPositions(_randomPositions);
            GetBoardPosition(board);

            SetBoardPosition(board, _randomPositions);

            SetBackImageActive(board, true);

            for (float k = 0.1f, i = 0.2f; i <= 4 || k <= 1f; i += 0.2f, k += 0.035f)
            {
                for (int j = 0; j < board.Count; j++)
                {
                    for (int z = 0; z < board[0].Count; z++)
                    {
                        board[j][z].BackGroundImage.pixelsPerUnitMultiplier = i;

                        if (board[j][z].View != null)
                            board[j][z].View.GetComponent<RectTransform>().anchoredPosition = //Ну а как иначе?)
                                new Vector2(
                                    Mathf.SmoothStep(_randomPositions[j, z].x, _boardPositions[j, z].x,
                                        EaseInOutQuint(k)),
                                    Mathf.SmoothStep(_randomPositions[j, z].y, _boardPositions[j, z].y,
                                        EaseInOutQuint(k)));
                    }
                }

                yield return new WaitForSeconds(0.07f);
            }

            foreach (var item in from row in board from item in row where item.View != null select item)
            {
                item.View.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(0, 0);
            }
        }

        private float EaseInOutQuint(float x)
        {
            return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
        }

        private void SetBoardPosition(List<List<BoardItemWidget>> board, Vector2[,] position)
        {
            for (var j = 0; j < board.Count; j++)
            {
                for (var z = 0; z < board[0].Count; z++)
                {
                    if (board[j][z].View != null)
                        board[j][z].View.GetComponent<RectTransform>().anchoredPosition =
                            position[j, z];
                }
            }
        }

        private void SetBackImageActive(List<List<BoardItemWidget>> board, bool value)
        {
            foreach (var boardItemWidget in board.SelectMany(b => b))
            {
                boardItemWidget.BackGroundImage.gameObject.SetActive(value);
            }
        }

        private void GetBoardPosition(List<List<BoardItemWidget>> board)
        {
            for (int i = 0; i < _boardPositions.GetLength(0); i++)
            for (int j = 0; j < _boardPositions.GetLength(1); j++)
            {
                if (board[i][j].View != null)
                    _boardPositions[i, j] = board[i][j].View.GetComponent<RectTransform>().anchoredPosition;
            }
        }

        private void FullRandomPositions(Vector2[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = GetRandomPosition();
            }
        }

        public IEnumerator Exiting(List<List<BoardItemWidget>> board)
        {
            _randomPositions ??= new Vector2[board.Count, board[0].Count];
            _boardPositions ??= new Vector2[board.Count, board[0].Count];

            FullRandomPositions(_randomPositions);
            GetBoardPosition(board);

            SetBoardPosition(board, _boardPositions);

            for (float k = 0.1f, i = 4f; i >= 0.2f || k <= 1f; i -= 0.2f, k += 0.05f)
            {
                for (int j = 0; j < board.Count; j++)
                {
                    for (int z = 0; z < board[0].Count; z++)
                    {
                        board[j][z].BackGroundImage.pixelsPerUnitMultiplier = i;

                        if (board[j][z].View != null)
                            board[j][z].View.GetComponent<RectTransform>().anchoredPosition = //Ну а как иначе?)
                                new Vector2(
                                    Mathf.SmoothStep(_boardPositions[j, z].x, _randomPositions[j, z].x,
                                        EaseInOutQuint(k)),
                                    Mathf.SmoothStep(_boardPositions[j, z].y, _randomPositions[j, z].y,
                                        EaseInOutQuint(k)));
                    }
                }

                yield return new WaitForSeconds(0.07f);
            }
            
            SetBackImageActive(board, false);
        }

        private Vector2 GetRandomPosition() //Все относительно => эти позиции выбираются относительно нашей сцены,
            //расширять функционал этого не требуется на самом деле
        {
            var isPositive = Random.Range(0, 101) > 50 ? 1 : -1;
            var x = Random.Range(_outLevelOffset, _outLevelOffset * 2) * isPositive;

            isPositive = Random.Range(0, 101) > 50 ? 1 : -1;
            var y = Random.Range(_outLevelOffset, _outLevelOffset * 2) * isPositive;

            return new Vector2(x, y);
        }
    }
}