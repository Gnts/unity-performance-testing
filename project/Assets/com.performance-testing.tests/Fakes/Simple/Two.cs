using System.Collections;
using System.Collections.Generic;
using PerformanceTesting;
using UnityEngine;

namespace PerformanceTesting.Tests
{
    public class Two
    {
        [Test]
        public void Void()
        {

        }

        [Test]
        public IEnumerator Enumerator()
        {
            Debug.Log("Some crazy log");
            yield return null;
        }

        [Setup]
        public void Setup_Void()
        {

        }

        [Setup]
        public IEnumerator Setup_Enumerator()
        {
            yield return null;
        }
    }
}