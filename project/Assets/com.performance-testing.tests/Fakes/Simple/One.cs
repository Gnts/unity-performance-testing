using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PerformanceTesting;

namespace PerformanceTesting.Tests.Simple
{

    public class One
    {
        [Test]
        public void Void()
        {

        }

        [Test]
        public IEnumerator Enumerator()
        {
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
