using System;
using VideoOS.Platform.DriverFramework.Definitions;

namespace DemoDriver
{
    public static class Constants
    {
        public static readonly Guid HardwareId = new Guid("22273EA2-AEFE-40ED-BBA5-49BAE4C11111");

        public static readonly ProductDefinition Product1 = new ProductDefinition
        {
            Id = new Guid("F7B0084B-497D-4B0E-9B14-3B0D443CF95D"),
            Name = "My Demo"
        };


        public static readonly Guid Audio1 = new Guid("F5F6C3B0-64B8-4208-9864-9104C9F72882");
        public static readonly Guid Camera1 = new Guid("5D954A98-242E-47C0-9DFD-73C323A73222");
        public static readonly Guid Camera2 = new Guid("ED298EE4-16C2-4A5F-A6D1-623A1D1AD811");
        public static readonly Guid Input1 = new Guid("D9B30A4A-618D-4DA9-A32F-7CD29E1A4DC7");
        public static readonly Guid Input2 = new Guid("C2748C5A-537F-4626-BA91-459EB9125E98");
        public static readonly Guid Metadata1 = new Guid("8A2E3122-A814-41CD-A280-64C65BADA835");
        public static readonly Guid Metadata2 = new Guid("2F2D37B0-8801-43F7-9EF9-F33BC7132375");
        public static readonly Guid Output1 = new Guid("8B0ECA3D-8A13-4EAE-9295-179A91DA69F6");
        public static readonly Guid Speaker1 = new Guid("FC4B0CD9-11D6-4E72-8A9A-9949767FCD43");

        public static readonly Guid RebootReferenceId = new Guid("142AF37A-EB54-4F2D-8D02-5BAE92661D86");
        public static readonly Guid ResourceIssueReferenceId = new Guid("96DDC265-E25B-4ADF-B0BF-6CCCAD165511");
        public static readonly Guid BandwidthLimitRefId = new Guid("31F9CF72-B304-4D3B-A281-99284D8E4265");
        public static readonly Guid AnalyticsEventReferenceId = new Guid("80CD871E-CE06-4000-BCAE-08A8323AB1AC");
        public static readonly Guid ResourceAnalyticsEventNameReferenceId = new Guid("0B5075FD-98A0-4D7B-9CFB-7B1CD5AC19DC");
        public static readonly Guid MotionStartedEventNameReferenceId = new Guid("3E70039B-6998-49FC-A145-F37425251D0E");
        public static readonly Guid MotionStoppedEventNameReferenceId = new Guid("0BF2ADF1-A432-47D7-A4C1-36A4B5295A6A");
        public static readonly Guid SoundActivatedEventReferenceId = new Guid("BF848E2D-44BA-4242-94CF-51C379B1F761");
        public static readonly Guid SoundDeactivatedEventReferenceId = new Guid("B5F00CCB-142F-44CC-AAFB-1D996797355B");
        public static readonly Guid MetadataInputActivated = new Guid("28B62600-BC3C-46FC-8E65-DA1F48DA4998");
        public static readonly Guid MetadataInputDeactivated = new Guid("C480E40D-AB06-4520-94E1-AA76AFFEB120");
        public static readonly Guid ResourceSoundActivatedEventNameReferenceId = new Guid("275420C3-6093-446C-BEA2-2E8CA7EA1170");
        public static readonly Guid ResourceSoundDeactivatedEventNameReferenceId = new Guid("AFDEAA2B-3EA0-4288-A886-7E2B6EC81917");
        public static readonly Guid ResourceInputActivatedEventNameReferenceId = new Guid("53CEC1C4-E06A-468F-8C8E-F0E3F7010AC9");
        public static readonly Guid ResourceInputDeactivatedEventNameReferenceId = new Guid("7BB60311-6772-4D59-A519-BB78620AE42A");

        public static readonly Guid CodecRefId = new Guid("1D7711B4-B278-487B-A0D1-5C9621D3885B");
        public static readonly Guid FPSRefId = new Guid("6527C12A-06F1-4F58-A2FE-6640C61707E0");
        public static readonly Guid CodecH264ReferenceId = new Guid("48FA594E-5EDC-47D8-9B5E-9891945CA360");
        public static readonly Guid CodecH264DisplayNameReferenceId = new Guid("D4FFCE76-A3EF-45FE-876A-ED9121934F18");
        public static readonly Guid CodecMjpegReferenceId = new Guid("32EFFDA2-FA6C-4253-A0D3-21F5404078D3");
        public static readonly Guid CodecMjpegDisplayNameReferenceId = new Guid("76C84C36-D7F8-47FE-9CD2-8FFB1EF85760");

        public static readonly Guid StringFieldRefId = new Guid("1BE62F5F-410A-4B3E-9456-1438C37A80CA");
        public static readonly Guid BoolFieldRefId = new Guid("9637B426-026D-49C5-B765-87DC4602DC9F");
        public static readonly Guid RotationFieldRefId = new Guid("203F2F1D-61D2-4FF6-B846-451FC3CC56FD");
        public static readonly Guid RotationFieldRefId0 = new Guid("76827014-63A7-4173-9187-97552EC87291");
        public static readonly Guid RotationFieldRefId90 = new Guid("6540DBE2-DF71-4D89-A8B7-C81A6E70EBB8");
        public static readonly Guid RotationFieldRefId180 = new Guid("FFE6E17B-E8AD-4F30-9941-FBE5E2BCD299");
        public static readonly Guid RotationFieldRefId270 = new Guid("A3EB3727-3042-46D1-A72A-5BFD7732368E");

        public static readonly Guid Stream1RefId = new Guid("CB618238-7503-49D2-A8F8-6DB8A8BFE4FB");
        public static readonly Guid Stream2RefId = new Guid("F5362140-3837-49F5-9AC7-BA1859AD4611");

        public static readonly Guid MetadataColorRefId = new Guid("FC1CFC08-F04A-431C-A8C2-F4F6905E2A0D");
        public static readonly Guid MetadataColorCRefId = new Guid("E4D03AE9-276C-4023-8913-6B0A89970D7E");
        public static readonly Guid MetadataColorBWRefId = new Guid("C4137C34-5182-4341-B13F-8A85F90576B5");

        public static readonly Guid AudioStream1RefId = new Guid("E637005E-2E92-4F4A-8DE8-D6367467DC34");
        public static readonly Guid SpeakerStream1RefId = new Guid("536E9925-BA7A-45F9-9188-DB32FC1A63BE");
        public static readonly Guid InputGainRefId = new Guid("D5F3D9D9-6C99-47CF-A986-E087C4322887");
        public static readonly Guid OutputGainRefId = new Guid("B323989F-6C12-45D4-8301-63B389BAD45E");

        public static readonly string BandwidthLimit = nameof(BandwidthLimit);
        public static readonly string Rotation = nameof(Rotation);
        public static readonly string BoundingBoxColor = nameof(BoundingBoxColor);
        public static readonly string FPS = nameof(FPS);
        public static readonly string Codec = nameof(Codec);
        public static readonly string SomeField = nameof(SomeField);
        public static readonly string BoolField = nameof(BoolField);
        public static readonly string InputGain = nameof(InputGain);
        public static readonly string OutputGain = nameof(OutputGain);

    }
}
