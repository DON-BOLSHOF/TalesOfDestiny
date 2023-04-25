using System;
using System.Collections;
using UnityEngine;

namespace CodeAnimation
{
    public class CameraAnimation
    {
        private float _baseApproximation;
        private float _estrangeApproximation;
        private float _currentApproximation = 1;

        public CameraAnimation(float baseApproximation, float estrangeApproximation)
        {
            _baseApproximation = baseApproximation;
            _estrangeApproximation = estrangeApproximation;
        }
        
        public IEnumerator Approximating(Camera camera, ApproximationType approximationType)
        {
            _currentApproximation = 1 - _currentApproximation;
            switch (approximationType)
            {
                case ApproximationType.Approximate:
                    for (var i = _currentApproximation; i <= 1; i += 0.05f, _currentApproximation += 0.05f)
                    {
                        camera.fieldOfView = Mathf.Lerp(_baseApproximation, _estrangeApproximation, i);
                        yield return new WaitForSeconds(0.07f);
                    }
                    break;
                case ApproximationType.Estrange:
                    for (var i = _currentApproximation; i <= 1; i += 0.05f , _currentApproximation += 0.05f)
                    {
                        camera.fieldOfView = Mathf.Lerp(_estrangeApproximation, _baseApproximation, i);
                        yield return new WaitForSeconds(0.07f);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(approximationType), approximationType, null);
            }
        }
    }
}

public enum ApproximationType
{
    Approximate,
    Estrange
}