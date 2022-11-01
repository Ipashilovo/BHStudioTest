using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Mirror;
using UnityEngine;

namespace UI
{
    public class ScoreScreen : MonoBehaviour
    {
        private Dictionary<string, ScoreView> _scoreViews = new Dictionary<string, ScoreView>();
        private Queue<ScoreView> _unactiveView = new Queue<ScoreView>();
        [SerializeField] private ScoreView _prefab;
        [SerializeField] private Transform _content;
        [SerializeField] private ScoreHandler _scoreHandler;
        [SerializeField] private WinScreen _winScreen;
        
        private void Start()
        {
            var scores = _scoreHandler.GetScore();
            foreach (var score in scores)
            {
                OnScoreChanged(score.Key, score.Value);
            }
            _scoreHandler.ScoreChanged += OnScoreChanged;
            _scoreHandler.FindedWinner += ShowWinScreen;
            _scoreHandler.RemovedClient += OnRemoveClient;
        }

        private void OnDestroy()
        {
            _scoreHandler.ScoreChanged -= OnScoreChanged;
            _scoreHandler.FindedWinner -= ShowWinScreen;
            _scoreHandler.RemovedClient -= OnRemoveClient;
        }

        private void OnRemoveClient(string obj)
        {
            if (_scoreViews.TryGetValue(obj, out var view))
            {
                view.gameObject.SetActive(false);
                _unactiveView.Enqueue(view);
                _scoreViews.Remove(obj);
            }
        }

        private void ShowWinScreen(string obj)
        {
            _winScreen.Init(obj);
            StartCoroutine(WaitRestart());
        }

        private IEnumerator WaitRestart()
        {
            yield return new WaitWhile(() => _scoreHandler.IsEnded);
            _winScreen.gameObject.SetActive(false);
        }

        private void OnScoreChanged(string id, int value)
        {
            if (_scoreViews.TryGetValue(id, out var view) == false)
            {
                var scoreView = Get();
                scoreView.Init(id);
                _scoreViews[id] = scoreView;
            }

            _scoreViews[id].Bind(value);
        }

        private ScoreView Get()
        {
            if (_unactiveView.TryDequeue(out var view))
            {
                return view;
            }

            return Instantiate(_prefab, _content);
        }
    }
}