using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Conf;
using System;

namespace Core.Gameplay
{
    public class Suitcase : MonoBehaviour, ICase
    {
        public SuitcasePart mainPart;
        private List<SuitcasePart> parts = new List<SuitcasePart>();
        private List<SuitcaseConnector> connectors = new List<SuitcaseConnector>();

        private GameObject GoodResult;
        private GameObject BadResult;

        private void Awake()
        {
            GoodResult = Resources.Load<GameObject>(Paths.GoodResult);
            BadResult = Resources.Load<GameObject>(Paths.BadResult);
            foreach (var part in GetComponentsInChildren<SuitcasePart>())
            {
                parts.Add(part);
            }

            foreach (var connector in GetComponents<SuitcaseConnector>())
            {
                connectors.Add(connector);
            }
        }

        public void Close()
        {
            StartCoroutine("StartCloseProcces");
        }

        public bool PlaceItem(Vector3 pos, GameObject item)
        {
            ItemData data = item.GetComponent<ItemData>();

            foreach (var part in parts)
            {
                if (part.PlaceItem(pos, data))
                {
                    SetPositionInPart(part, item);
                    return true;
                }
            }
            return false;
        }

        public bool PlaceItem(int x, int y, GameObject item, SuitcasePart part)
        {
            ItemData data = item.GetComponent<ItemData>();
            if (part.PlaceItem(x, y, data))
            {
                SetPositionInPart(part, item);
                return true;
            }
            return false;
        }

        public bool PlaceItem(int x, int y, GameObject item)
        {
            return PlaceItem(x, y, item, parts[0]);
        }

        public GameObject TakeItem(Vector3 pos)
        {
            ItemData data;
            foreach (var part in parts)
            {
                data = part.DeleteItem(pos);
                if (data != null)
                {
                    GameObject go = data.gameObject;
                    go.transform.SetParent(null);
                    return go;
                }
            }
            return null;
        }

        private void SetPositionInPart(SuitcasePart part, GameObject item)
        {
            ItemData data = item.GetComponent<ItemData>();
            float GridSize = part.GetGridSize();
            Vector2 coord = data.Anchor;
            Vector2 size = new Vector2(data.Config.width, data.Config.height);
            item.transform.SetParent(part.transform.GetChild(0));
            item.transform.localPosition = new Vector3(GridSize / 2 * size.x + coord.x * GridSize, GridSize / 2 * size.y + coord.y * GridSize, part.OffsetZ);
        }

        private void ShowResult(int x, int y, bool correct)
        {
            GameObject go;
            float GridSize = mainPart.GetGridSize();
            if (correct)
            {
                go = Instantiate(GoodResult, mainPart.transform);
            }
            else
            {
                go = Instantiate(BadResult, mainPart.transform);
            }
            go.transform.localPosition = new Vector3(GridSize / 2 + x * GridSize, GridSize / 2 + y * GridSize, mainPart.OffsetZ - 2f);
        }

        private List<SuitcaseConnector> FindNextConnectors()
        {
            List<SuitcaseConnector> result = new List<SuitcaseConnector>();
            int? CurrentPriority = null;

            foreach (var connector in connectors)
            {
                if (connector.isOpen && (CurrentPriority == null || connector.priority >= CurrentPriority))
                {
                    if (connector.priority != CurrentPriority)
                    {
                        CurrentPriority = connector.priority;
                        result = new List<SuitcaseConnector>();
                    }
                    result.Add(connector);
                }
            }
            if (CurrentPriority != null)
            {
                return result;
            }
            return null;
        }

        private bool CheckCrossing()
        {
            int[] result = new int[parts[0].slots.Capacity];

            foreach (var part in parts)
            {
                for (int i = 0; i < part.Width; i++)
                {
                    for (int j = 0; j < part.Height; j++)
                    {
                        if (part.FlipX && part.FlipY)
                        {
                            if (part.slots[(part.Width - 1) - i, (part.Height - 1) - j].itemData != null)
                            {
                                result[i + j * part.Width]++;
                            }
                        }
                        else if (part.FlipX)
                        {
                            if (part.slots[(part.Width - 1) - i, j].itemData != null)
                            {
                                result[i + j * part.Width]++;
                            }
                        }
                        else if (part.FlipY)
                        {
                            if (part.slots[i, (part.Height - 1) - j].itemData != null)
                            {
                                result[i + j * part.Width]++;
                            }
                        }
                        else
                        {
                            if (part.slots[i, j].itemData != null)
                            {
                                result[i + j * part.Width]++;
                            }
                        }
                    }
                }
            }

            bool WinFlag = true;
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] > 1)
                {
                    WinFlag = false;
                    ShowResult(i % parts[0].Width, i / parts[0].Width, false);
                }
                else
                {
                    ShowResult(i % parts[0].Width, i / parts[0].Width, true);
                }

            }

            return WinFlag;
        }

        IEnumerator StartCloseProcces()
        {
            while (true)
            {
                List<SuitcaseConnector> OpeningConnectors = FindNextConnectors();
                if (OpeningConnectors == null) break;
                foreach (var connector in OpeningConnectors)
                {
                    connector.Close();
                }
                yield return new WaitForSeconds(0.5f);
            }
            yield return ShowResult();
        }

        IEnumerator ShowResult()
        {
            if (CheckCrossing())
            {
                if(GameStateController.instance.HasVibrate) Handheld.Vibrate();
                yield return new WaitForSeconds(1f);
                GameStateController.instance.WinGame();
            }
            else
            {
                if (GameStateController.instance.HasVibrate) Handheld.Vibrate();
                yield return new WaitForSeconds(1f);
                GameStateController.instance.LoseGame();
            }
            yield break;
        }
    }
}

