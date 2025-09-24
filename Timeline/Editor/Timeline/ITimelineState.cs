using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200000D RID: 13
	internal interface ITimelineState
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000076 RID: 118
		// (set) Token: 0x06000077 RID: 119
		bool timeInFrames { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000078 RID: 120
		// (set) Token: 0x06000079 RID: 121
		Vector2 playRangeTime { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600007A RID: 122
		bool canRecord { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600007B RID: 123
		bool recording { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600007C RID: 124
		// (set) Token: 0x0600007D RID: 125
		bool playing { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600007E RID: 126
		// (set) Token: 0x0600007F RID: 127
		bool previewMode { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000080 RID: 128
		// (set) Token: 0x06000081 RID: 129
		float playbackSpeed { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000082 RID: 130
		// (set) Token: 0x06000083 RID: 131
		float frameRate { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000084 RID: 132
		// (set) Token: 0x06000085 RID: 133
		double time { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000086 RID: 134
		double localStart { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000087 RID: 135
		// (set) Token: 0x06000088 RID: 136
		int frame { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000089 RID: 137
		TimelineAsset timeline { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600008A RID: 138
		TimelineAsset rootTimeline { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600008B RID: 139
		TrackAsset rootTrack { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600008C RID: 140
		EditorWindow editorWindow { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600008D RID: 141
		// (set) Token: 0x0600008E RID: 142
		PlayableDirector currentDirector { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600008F RID: 143
		// (set) Token: 0x06000090 RID: 144
		bool rebuildGraph { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000091 RID: 145
		// (set) Token: 0x06000092 RID: 146
		bool restoreSelection { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000093 RID: 147
		// (set) Token: 0x06000094 RID: 148
		ObjectPickerCallback ObjectPickerSelectionCallback { get; set; }

		// Token: 0x06000095 RID: 149
		void Refresh();

		// Token: 0x06000096 RID: 150
		float TimeToTimeAreaPixel(double time);

		// Token: 0x06000097 RID: 151
		float TimeToScreenSpacePixel(double time);

		// Token: 0x06000098 RID: 152
		float TimeToPixel(double time);

		// Token: 0x06000099 RID: 153
		float PixelToTime(float pixel);

		// Token: 0x0600009A RID: 154
		string TimeAsString(double timeValue, string format);

		// Token: 0x0600009B RID: 155
		float TimeAreaPixelToTime(float pixel);

		// Token: 0x0600009C RID: 156
		float ScreenSpacePixelToTimeAreaTime(float pixel);

		// Token: 0x0600009D RID: 157
		Component GetBindingForTrack(TrackAsset trackAsset);

		// Token: 0x0600009E RID: 158
		void AddEndFrameDelegate(PendingUpdateDelegate callback);

		// Token: 0x0600009F RID: 159
		void SetCurrentSequence(TimelineAsset asset);

		// Token: 0x060000A0 RID: 160
		double SnapToFrameIfRequired(double time);
	}
}
