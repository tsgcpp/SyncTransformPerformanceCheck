using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestPhysicsSyncTiming
{
    Builder _builder;

    bool _lastAutoSyncTransforms;

    [OneTimeSetUp]
    public void SetUpFixture()
    {
        _lastAutoSyncTransforms = Physics.autoSyncTransforms;
    }

    [SetUp]
    public void SetUp()
    {
        Physics.autoSyncTransforms = false;
        _builder = new Builder();
    }

    [TearDown]
    public void TearDown()
    {
        _builder.Dispose();
        Physics.autoSyncTransforms = _lastAutoSyncTransforms;
    }

    [UnityTest]
    public IEnumerator SetUp_RaycastReturnsTrue()
    {
        yield return null;

        // when
        _builder
            .WhenColliderPositionIs(CollidedPosition)
            .WhenColliderTriggerAs(false)
            .Build();

        /*
         FYI:
         In https://docs.unity3d.com/Manual/ExecutionOrder.html,
         "yield WaitForFixedUpdate" is after "Internal physics update".
         "Internal physics update" will flush all colliders to the physics engine.
         */

        // flush the collider to the physics engine
        yield return new WaitForFixedUpdate();

        // then
        _builder.VerifyRaycastCollided(origin: Vector3.zero, direction: Vector3.forward);
    }

    [UnityTest]
    public IEnumerator Raycast_ColliderDoesNotMoveAfterPositionChanged()
    {
        yield return null;

        // setup
        _builder
            .WhenColliderPositionIs(CollidedPosition)
            .WhenColliderTriggerAs(false)
            .Build();
        yield return new WaitForFixedUpdate();

        // when
        _builder.WhenColliderPositionIs(NotCollidedPosition);

        // then
        _builder.VerifyRaycastCollided(origin: Vector3.zero, direction: Vector3.forward);
    }

    [UnityTest]
    public IEnumerator Raycast_ColliderDoesNotMoveAfterPositionChanged_AsTrigger()
    {
        yield return null;

        // setup
        _builder
            .WhenColliderPositionIs(CollidedPosition)
            .WhenColliderTriggerAs(true)
            .Build();
        yield return new WaitForFixedUpdate();

        // when
        _builder.WhenColliderPositionIs(NotCollidedPosition);

        // then
        _builder.VerifyRaycastCollided(origin: Vector3.zero, direction: Vector3.forward);
    }

    [UnityTest]
    public IEnumerator Raycast_ColliderMovesAfterPositionChangedAndFixedUpdate()
    {
        yield return null;

        // setup
        _builder
            .WhenColliderPositionIs(CollidedPosition)
            .WhenColliderTriggerAs(false)
            .Build();
        yield return new WaitForFixedUpdate();

        // when
        _builder.WhenColliderPositionIs(NotCollidedPosition);
        yield return new WaitForFixedUpdate();

        // then
        _builder.VerifyRaycastDoesNotCollided(origin: Vector3.zero, direction: Vector3.forward);
    }

    [UnityTest]
    public IEnumerator Raycast_ColliderMovesAfterPositionChangedAndFixedUpdate_AsTrigger()
    {
        yield return null;

        // setup
        _builder
            .WhenColliderPositionIs(CollidedPosition)
            .WhenColliderTriggerAs(true)
            .Build();
        yield return new WaitForFixedUpdate();

        // when
        _builder.WhenColliderPositionIs(NotCollidedPosition);
        yield return new WaitForFixedUpdate();

        // then
        _builder.VerifyRaycastDoesNotCollided(origin: Vector3.zero, direction: Vector3.forward);
    }

    [UnityTest]
    public IEnumerator Raycast_ColliderMovesAfterSyncTransform()
    {
        yield return null;

        // setup
        _builder
            .WhenColliderPositionIs(CollidedPosition)
            .WhenColliderTriggerAs(false)
            .Build();
        yield return new WaitForFixedUpdate();

        // when
        _builder.WhenColliderPositionIs(NotCollidedPosition);
        Physics.SyncTransforms();

        // then
        _builder.VerifyRaycastDoesNotCollided(origin: Vector3.zero, direction: Vector3.forward);
    }

    [UnityTest]
    public IEnumerator Raycast_ColliderMovesAfterSyncTransform_AsTrigger()
    {
        yield return null;

        // setup
        _builder
            .WhenColliderPositionIs(CollidedPosition)
            .WhenColliderTriggerAs(true)
            .Build();
        yield return new WaitForFixedUpdate();

        // when
        _builder.WhenColliderPositionIs(NotCollidedPosition);
        Physics.SyncTransforms();

        // then
        _builder.VerifyRaycastDoesNotCollided(origin: Vector3.zero, direction: Vector3.forward);
    }

    [UnityTest]
    public IEnumerator Raycast_ColliderMovesAfterPositionChangedAndFixedUpdate_RaycastToNextPosition()
    {
        yield return null;

        // setup
        _builder
            .WhenColliderPositionIs(CollidedPosition)
            .WhenColliderTriggerAs(false)
            .Build();
        yield return new WaitForFixedUpdate();

        // when
        _builder.WhenColliderPositionIs(NotCollidedPosition);
        yield return new WaitForFixedUpdate();

        // then
        _builder.VerifyRaycastCollided(origin: Vector3.zero, direction: Vector3.back);
    }

    [UnityTest]
    public IEnumerator Raycast_ColliderMovesAfterPositionChangedAndFixedUpdate_RaycastToNextPosition_AsTrigger()
    {
        yield return null;

        // setup
        _builder
            .WhenColliderPositionIs(CollidedPosition)
            .WhenColliderTriggerAs(true)
            .Build();
        yield return new WaitForFixedUpdate();

        // when
        _builder.WhenColliderPositionIs(NotCollidedPosition);
        yield return new WaitForFixedUpdate();

        // then
        _builder.VerifyRaycastCollided(origin: Vector3.zero, direction: Vector3.back);
    }

    [UnityTest]
    public IEnumerator Raycast_ColliderDoesNotMoveIfGameObjectIsDisabled()
    {
        yield return null;

        // setup
        _builder
            .WhenColliderPositionIs(CollidedPosition)
            .WhenColliderObjectActiveIs(false)
            .Build();
        yield return new WaitForFixedUpdate();

        // when
        _builder.WhenColliderPositionIs(NotCollidedPosition);
        Physics.SyncTransforms();

        _builder.WhenColliderObjectActiveIs(true);

        // then
        _builder.VerifyRaycastDoesNotCollided(origin: Vector3.zero, direction: Vector3.forward);
    }

    [UnityTest]
    public IEnumerator Raycast_ColliderDoesNotMoveIfGameObjectIsDisabled_AsTrigger()
    {
        yield return null;

        // setup
        _builder
            .WhenColliderPositionIs(CollidedPosition)
            .WhenColliderObjectActiveIs(false)
            .WhenColliderTriggerAs(true)
            .Build();
        yield return new WaitForFixedUpdate();

        // when
        _builder.WhenColliderPositionIs(NotCollidedPosition);
        Physics.SyncTransforms();

        _builder.WhenColliderObjectActiveIs(true);

        // then
        _builder.VerifyRaycastDoesNotCollided(origin: Vector3.zero, direction: Vector3.forward);
    }

    [UnityTest]
    public IEnumerator Raycast_ColliderDoesNotMoveIfColliderEnabledIsFalse()
    {
        yield return null;

        // setup
        _builder
            .WhenColliderPositionIs(CollidedPosition)
            .WhenColliderComponentEnabledIs(false)
            .Build();
        yield return new WaitForFixedUpdate();

        // when
        _builder.WhenColliderPositionIs(NotCollidedPosition);
        Physics.SyncTransforms();

        _builder.WhenColliderComponentEnabledIs(true);

        // then
        _builder.VerifyRaycastDoesNotCollided(origin: Vector3.zero, direction: Vector3.forward);
    }

    [UnityTest]
    public IEnumerator Raycast_ColliderDoesNotMoveIfColliderEnabledIsFalse_AsTrigger()
    {
        yield return null;

        // setup
        _builder
            .WhenColliderPositionIs(CollidedPosition)
            .WhenColliderComponentEnabledIs(false)
            .WhenColliderTriggerAs(true)
            .Build();
        yield return new WaitForFixedUpdate();

        // when
        _builder.WhenColliderPositionIs(NotCollidedPosition);
        Physics.SyncTransforms();

        _builder.WhenColliderComponentEnabledIs(true);

        // then
        _builder.VerifyRaycastDoesNotCollided(origin: Vector3.zero, direction: Vector3.forward);
    }

    public class Builder : IDisposable
    {
        public BoxCollider TargetCollider { get; }

        public Builder()
        {
            var go = new GameObject("TestPhysicsSyncTiming._targetCollider");
            TargetCollider = go.AddComponent<BoxCollider>();
            TargetCollider.size = ColliderSize;
        }

        public BoxCollider Build()
        {
            return TargetCollider;
        }

        public void Dispose()
        {
            UnityEngine.Object.DestroyImmediate(TargetCollider.gameObject);
        }

        public Builder WhenColliderPositionIs(Vector3 position)
        {
            TargetCollider.gameObject.transform.position = position;
            return this;
        }

        public Builder WhenColliderTriggerAs(bool isTrigger)
        {
            TargetCollider.isTrigger = isTrigger;
            return this;
        }

        public Builder WhenColliderComponentEnabledIs(bool value)
        {
            TargetCollider.enabled = value;
            return this;
        }

        public Builder WhenColliderObjectActiveIs(bool value)
        {
            TargetCollider.gameObject.SetActive(value);
            return this;
        }

        public void VerifyRaycastCollided(Vector3 origin, Vector3 direction)
        {
            Assert.True(Raycast(origin, direction, out var hit));
            Assert.That(hit.collider, Is.EqualTo(TargetCollider));
        }

        public void VerifyRaycastDoesNotCollided(Vector3 origin, Vector3 direction)
        {
            Assert.False(Raycast(origin, direction, out _));
        }

        private bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hit)
        {
            return Physics.Raycast(
                ray: new Ray(origin: origin, direction: direction),
                out hit,
                maxDistance: MaxDistance);
        }

        const float MaxDistance = 100f;
        readonly Vector3 ColliderSize = new Vector3(0.001f, 0.001f, 0.001f);
    }

    readonly Vector3 CollidedPosition = new Vector3(0f, 0f, 3f);
    readonly Vector3 NotCollidedPosition = new Vector3(0f, 0f, -1f);
}
