using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200002F RID: 47
	internal class TimelineAnimationUtilities
	{
		// Token: 0x060001C5 RID: 453 RVA: 0x000108DC File Offset: 0x0000ECDC
		public static bool ValidateOffsetAvailabitity(PlayableDirector director, Animator animator)
		{
			return !(director == null) && !(animator == null);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00010914 File Offset: 0x0000ED14
		public static TimelineClip GetPreviousClip(TimelineClip clip)
		{
			TimelineClip timelineClip = null;
			foreach (TimelineClip timelineClip2 in clip.parentTrack.clips)
			{
				if (timelineClip2.start < clip.start && (timelineClip == null || timelineClip2.start >= timelineClip.start))
				{
					timelineClip = timelineClip2;
				}
			}
			return timelineClip;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00010980 File Offset: 0x0000ED80
		public static TimelineClip GetNextClip(TimelineClip clip)
		{
			return (from c in clip.parentTrack.clips
			where c.start > clip.start
			orderby c.start
			select c).FirstOrDefault<TimelineClip>();
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000109EC File Offset: 0x0000EDEC
		public static void ComputeClipWorldSpaceOffset(PlayableDirector director, TimelineClip clip, out Vector3 clipPositionOffset, out Quaternion clipRotationOffset)
		{
			clipPositionOffset = Vector3.zero;
			clipRotationOffset = Quaternion.identity;
			GameObject sceneGameObject = TimelineUtility.GetSceneGameObject(director, clip.parentTrack);
			double time = director.time;
			AnimationTrack animationTrack = clip.parentTrack as AnimationTrack;
			TimelineClip[] clips = animationTrack.clips;
			director.Stop();
			for (int i = 0; i < clips.Length; i++)
			{
				animationTrack.RemoveClip(clips[i]);
			}
			animationTrack.AddClip(clip);
			double start = clip.start;
			double blendInDuration = clip.blendInDuration;
			clip.blendInDuration = 0.0;
			clip.start = 1.0;
			director.Play();
			director.time = 0.0;
			director.Evaluate();
			clipPositionOffset = sceneGameObject.transform.position;
			clipRotationOffset = sceneGameObject.transform.rotation;
			AnimationPlayableAsset animationPlayableAsset = clip.asset as AnimationPlayableAsset;
			if (!(animationPlayableAsset == null))
			{
				clipPositionOffset += clipRotationOffset * animationPlayableAsset.position;
				clipRotationOffset *= animationPlayableAsset.rotation;
				director.Stop();
				clip.start = start;
				clip.blendInDuration = blendInDuration;
				animationTrack.RemoveClip(clip);
				for (int j = 0; j < clips.Length; j++)
				{
					animationTrack.AddClip(clips[j]);
				}
				director.Play();
				director.time = time;
				director.Evaluate();
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00010B84 File Offset: 0x0000EF84
		public static void ComputeTrackOffsets(PlayableDirector director, TimelineClip clip, out Vector3 parentPositionOffset, out Quaternion parentRotationOffset, out Vector3 positionOffset, out Quaternion rotationOffset)
		{
			positionOffset = Vector3.zero;
			rotationOffset = Quaternion.identity;
			parentPositionOffset = Vector3.zero;
			parentRotationOffset = Quaternion.identity;
			GameObject sceneGameObject = TimelineUtility.GetSceneGameObject(director, clip.parentTrack);
			double time = director.time;
			AnimationTrack animationTrack = clip.parentTrack as AnimationTrack;
			TimelineClip[] clips = animationTrack.clips;
			director.Stop();
			for (int i = 0; i < clips.Length; i++)
			{
				animationTrack.RemoveClip(clips[i]);
			}
			animationTrack.AddClip(clip);
			double start = clip.start;
			double blendInDuration = clip.blendInDuration;
			clip.blendInDuration = 0.0;
			clip.start = 1.0;
			director.Play();
			director.time = 1.0;
			director.Evaluate();
			positionOffset = sceneGameObject.transform.position;
			rotationOffset = sceneGameObject.transform.rotation;
			director.time = 0.0;
			director.Evaluate();
			parentPositionOffset = sceneGameObject.transform.position;
			parentRotationOffset = sceneGameObject.transform.rotation;
			director.Stop();
			clip.start = start;
			clip.blendInDuration = blendInDuration;
			animationTrack.RemoveClip(clip);
			for (int j = 0; j < clips.Length; j++)
			{
				animationTrack.AddClip(clips[j]);
			}
			director.Play();
			director.time = time;
			director.Evaluate();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00010D1C File Offset: 0x0000F11C
		public static TimelineAnimationUtilities.RigidTransform UpdateClipOffsets(AnimationPlayableAsset asset, AnimationTrack track, Transform transform, Vector3 globalPosition, Quaternion globalRotation)
		{
			Matrix4x4 worldToLocalMatrix = transform.worldToLocalMatrix;
			Matrix4x4 matrix4x = Matrix4x4.TRS(asset.position, asset.rotation, Vector3.one);
			Matrix4x4 matrix4x2 = Matrix4x4.TRS(track.position, track.rotation, Vector3.one);
			if (transform.parent != null)
			{
				matrix4x2 = transform.parent.localToWorldMatrix * matrix4x2;
			}
			Vector3 position = transform.position;
			Quaternion rotation = transform.rotation;
			transform.position = globalPosition;
			transform.rotation = globalRotation;
			Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
			transform.position = position;
			transform.rotation = rotation;
			Matrix4x4 matrix4x3 = matrix4x2.inverse * localToWorldMatrix * worldToLocalMatrix * matrix4x2 * matrix4x;
			return TimelineAnimationUtilities.RigidTransform.Compose(matrix4x3.GetColumn(3), MathUtils.QuaternionFromMatrix(matrix4x3));
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00010E00 File Offset: 0x0000F200
		public static TimelineAnimationUtilities.RigidTransform GetTrackOffsets(AnimationTrack track, Transform transform)
		{
			Vector3 vector = track.position;
			Quaternion quaternion = track.rotation;
			if (transform.parent != null)
			{
				vector = transform.parent.TransformPoint(vector);
				quaternion = transform.parent.rotation * quaternion;
				MathUtils.QuaternionNormalize(ref quaternion);
			}
			return TimelineAnimationUtilities.RigidTransform.Compose(vector, quaternion);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00010E64 File Offset: 0x0000F264
		public static void UpdateTrackOffset(AnimationTrack track, Transform transform, TimelineAnimationUtilities.RigidTransform offsets)
		{
			if (transform.parent != null)
			{
				offsets.position = transform.parent.InverseTransformPoint(offsets.position);
				offsets.rotation = Quaternion.Inverse(transform.parent.rotation) * offsets.rotation;
				MathUtils.QuaternionNormalize(ref offsets.rotation);
			}
			track.position = offsets.position;
			track.rotation = offsets.rotation;
			track.UpdateClipOffsets();
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00010EF0 File Offset: 0x0000F2F0
		private static MatchTargetFields GetMatchFields(TimelineClip clip)
		{
			AnimationTrack animationTrack = clip.parentTrack as AnimationTrack;
			MatchTargetFields result;
			if (animationTrack == null)
			{
				result = MatchTargetFieldConstants.None;
			}
			else
			{
				AnimationPlayableAsset animationPlayableAsset = clip.asset as AnimationPlayableAsset;
				MatchTargetFields matchTargetFields = animationTrack.matchTargetFields;
				if (animationPlayableAsset != null && !animationPlayableAsset.useTrackMatchFields)
				{
					matchTargetFields = animationPlayableAsset.matchTargetFields;
				}
				result = matchTargetFields;
			}
			return result;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00010F5C File Offset: 0x0000F35C
		private static void WriteMatchFields(AnimationPlayableAsset asset, TimelineAnimationUtilities.RigidTransform result, MatchTargetFields fields)
		{
			Vector3 position = asset.position;
			position.x = ((!MatchTargetFieldConstants.HasAny(fields, 1)) ? position.x : result.position.x);
			position.y = ((!MatchTargetFieldConstants.HasAny(fields, 2)) ? position.y : result.position.y);
			position.z = ((!MatchTargetFieldConstants.HasAny(fields, 4)) ? position.z : result.position.z);
			asset.position = position;
			if (MatchTargetFieldConstants.HasAny(fields, MatchTargetFieldConstants.Rotation))
			{
				Vector3 eulerAngles = asset.rotation.eulerAngles;
				Vector3 eulerAngles2 = result.rotation.eulerAngles;
				eulerAngles.x = ((!MatchTargetFieldConstants.HasAny(fields, 8)) ? eulerAngles.x : eulerAngles2.x);
				eulerAngles.y = ((!MatchTargetFieldConstants.HasAny(fields, 16)) ? eulerAngles.y : eulerAngles2.y);
				eulerAngles.z = ((!MatchTargetFieldConstants.HasAny(fields, 32)) ? eulerAngles.z : eulerAngles2.z);
				asset.rotation = Quaternion.Euler(eulerAngles);
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000110A8 File Offset: 0x0000F4A8
		public static void MatchPrevious(TimelineClip currentClip, Transform matchPoint, PlayableDirector director)
		{
			MatchTargetFields matchFields = TimelineAnimationUtilities.GetMatchFields(currentClip);
			if (matchFields != MatchTargetFieldConstants.None && !(matchPoint == null))
			{
				double time = director.time;
				TimelineClip previousClip = TimelineAnimationUtilities.GetPreviousClip(currentClip);
				if (previousClip != null && currentClip != previousClip)
				{
					AnimationTrack animationTrack = currentClip.parentTrack as AnimationTrack;
					double blendInDuration = currentClip.blendInDuration;
					currentClip.blendInDuration = 0.0;
					double blendOutDuration = previousClip.blendOutDuration;
					previousClip.blendOutDuration = 0.0;
					director.Stop();
					animationTrack.RemoveClip(currentClip);
					director.Play();
					double num = (currentClip.start <= previousClip.end) ? currentClip.start : previousClip.end;
					director.time = num - 1E-05;
					director.Evaluate();
					Vector3 position = matchPoint.position;
					Quaternion rotation = matchPoint.rotation;
					director.Stop();
					animationTrack.AddClip(currentClip);
					animationTrack.RemoveClip(previousClip);
					director.Play();
					director.time = currentClip.start + 1E-05;
					director.Evaluate();
					AnimationPlayableAsset asset = currentClip.asset as AnimationPlayableAsset;
					TimelineAnimationUtilities.RigidTransform result = TimelineAnimationUtilities.UpdateClipOffsets(asset, animationTrack, matchPoint, position, rotation);
					TimelineAnimationUtilities.WriteMatchFields(asset, result, matchFields);
					currentClip.blendInDuration = blendInDuration;
					previousClip.blendOutDuration = blendOutDuration;
					director.Stop();
					animationTrack.AddClip(previousClip);
					director.Play();
					director.time = time;
					director.Evaluate();
				}
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00011220 File Offset: 0x0000F620
		public static void MatchNext(TimelineClip currentClip, Transform matchPoint, PlayableDirector director)
		{
			MatchTargetFields matchFields = TimelineAnimationUtilities.GetMatchFields(currentClip);
			if (matchFields != MatchTargetFieldConstants.None && !(matchPoint == null))
			{
				double time = director.time;
				TimelineClip nextClip = TimelineAnimationUtilities.GetNextClip(currentClip);
				if (nextClip != null && currentClip != nextClip)
				{
					AnimationTrack animationTrack = currentClip.parentTrack as AnimationTrack;
					double blendOutDuration = currentClip.blendOutDuration;
					double blendInDuration = nextClip.blendInDuration;
					currentClip.blendOutDuration = 0.0;
					nextClip.blendInDuration = 0.0;
					director.Stop();
					animationTrack.RemoveClip(currentClip);
					director.Play();
					director.time = nextClip.start + 1E-05;
					director.Evaluate();
					Vector3 position = matchPoint.position;
					Quaternion rotation = matchPoint.rotation;
					director.Stop();
					animationTrack.AddClip(currentClip);
					animationTrack.RemoveClip(nextClip);
					director.Play();
					director.time = Math.Min(nextClip.start, currentClip.end - 1E-05);
					director.Evaluate();
					AnimationPlayableAsset asset = currentClip.asset as AnimationPlayableAsset;
					TimelineAnimationUtilities.RigidTransform result = TimelineAnimationUtilities.UpdateClipOffsets(asset, animationTrack, matchPoint, position, rotation);
					TimelineAnimationUtilities.WriteMatchFields(asset, result, matchFields);
					currentClip.blendOutDuration = blendOutDuration;
					nextClip.blendInDuration = blendInDuration;
					director.Stop();
					animationTrack.AddClip(nextClip);
					director.Play();
					director.time = time;
					director.Evaluate();
				}
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00011384 File Offset: 0x0000F784
		public static TimelineWindowTimeControl CreateTimeController(TimelineWindow.TimelineState state, TimelineClip clip)
		{
			AnimationWindow window = EditorWindow.GetWindow<AnimationWindow>();
			TimelineWindowTimeControl timelineWindowTimeControl = ScriptableObject.CreateInstance<TimelineWindowTimeControl>();
			timelineWindowTimeControl.Init(state.GetWindow(), window.state, clip);
			return timelineWindowTimeControl;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000113BC File Offset: 0x0000F7BC
		public static TimelineWindowTimeControl CreateTimeController(TimelineWindow.TimelineState state, TimelineWindowTimeControl.ClipData clipData)
		{
			AnimationWindow window = EditorWindow.GetWindow<AnimationWindow>();
			TimelineWindowTimeControl timelineWindowTimeControl = ScriptableObject.CreateInstance<TimelineWindowTimeControl>();
			timelineWindowTimeControl.Init(state.GetWindow(), window.state, clipData);
			return timelineWindowTimeControl;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000113F4 File Offset: 0x0000F7F4
		public static void EditAnimationClipWithTimeController(AnimationClip animationClip, TimelineWindowTimeControl timeController, Object sourceObject)
		{
			AnimationWindow window = EditorWindow.GetWindow<AnimationWindow>();
			window.EditSequencerClip(animationClip, sourceObject, timeController);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00011414 File Offset: 0x0000F814
		public static int GetAnimationWindowCurrentFrame()
		{
			AnimationWindow window = EditorWindow.GetWindow<AnimationWindow>();
			int result;
			if (window)
			{
				result = window.state.currentFrame;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0001144C File Offset: 0x0000F84C
		public static void SetAnimationWindowCurrentFrame(int frame)
		{
			AnimationWindow window = EditorWindow.GetWindow<AnimationWindow>();
			if (window)
			{
				window.state.currentFrame = frame;
			}
		}

		// Token: 0x02000030 RID: 48
		public enum OffsetEditMode
		{
			// Token: 0x0400018B RID: 395
			None = -1,
			// Token: 0x0400018C RID: 396
			Translation,
			// Token: 0x0400018D RID: 397
			Rotation
		}

		// Token: 0x02000031 RID: 49
		public struct RigidTransform
		{
			// Token: 0x060001D7 RID: 471 RVA: 0x00011494 File Offset: 0x0000F894
			public static TimelineAnimationUtilities.RigidTransform Compose(Vector3 pos, Quaternion rot)
			{
				TimelineAnimationUtilities.RigidTransform result;
				result.position = pos;
				result.rotation = rot;
				return result;
			}

			// Token: 0x060001D8 RID: 472 RVA: 0x000114BC File Offset: 0x0000F8BC
			public static TimelineAnimationUtilities.RigidTransform Mul(TimelineAnimationUtilities.RigidTransform a, TimelineAnimationUtilities.RigidTransform b)
			{
				TimelineAnimationUtilities.RigidTransform result;
				result.rotation = a.rotation * b.rotation;
				result.position = a.position + a.rotation * b.position;
				return result;
			}

			// Token: 0x060001D9 RID: 473 RVA: 0x00011514 File Offset: 0x0000F914
			public static TimelineAnimationUtilities.RigidTransform Inverse(TimelineAnimationUtilities.RigidTransform a)
			{
				TimelineAnimationUtilities.RigidTransform result;
				result.rotation = Quaternion.Inverse(a.rotation);
				result.position = result.rotation * -a.position;
				return result;
			}

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x060001DA RID: 474 RVA: 0x0001155C File Offset: 0x0000F95C
			public static TimelineAnimationUtilities.RigidTransform identity
			{
				get
				{
					return TimelineAnimationUtilities.RigidTransform.Compose(Vector3.zero, Quaternion.identity);
				}
			}

			// Token: 0x0400018E RID: 398
			public Vector3 position;

			// Token: 0x0400018F RID: 399
			public Quaternion rotation;
		}
	}
}
