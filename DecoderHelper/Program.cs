// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using DecoderHelper;
using LegoBluetooth;
using LegoBluetooth.Device;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;

Console.WriteLine("Hello, World!");

//DecodeStrings();
//DecodeByteArray();
HydrateForSerialization();

void DecodeStrings()
{
    //    string toDecode =
    //    @"-> 0F-00-04-00-01-27-00-00-00-00-10-00-00-00-10
    //-> 0F-00-04-01-01-27-00-00-00-00-10-00-00-00-10
    //-> 0F-00-04-02-01-25-00-00-00-00-10-00-00-00-10
    //-> 0F-00-04-03-01-26-00-00-00-00-10-00-00-00-10
    //-> 09-00-04-10-02-27-00-00-01
    //-> 0F-00-04-32-01-17-00-00-00-00-01-06-00-00-20
    //-> 0F-00-04-3A-01-28-00-00-00-00-10-00-00-01-02
    //-> 0F-00-04-3B-01-15-00-02-00-00-00-00-00-01-00
    //-> 0F-00-04-3C-01-14-00-02-00-00-00-00-00-01-00
    //-> 0F-00-04-46-01-42-00-01-00-00-00-00-00-00-10
    //0B-00-43-00-01-0F-03-06-00-07-00
    //07-00-43-00-02-06-00
    //11-00-44-00-00-00-50-4F-57-45-52-00-00-00-00-00-00
    //0E-00-44-00-00-01-00-00-C8-C2-00-00-C8-42
    //0E-00-44-00-00-02-00-00-C8-C2-00-00-C8-42
    //0E-00-44-00-00-03-00-00-C8-C2-00-00-C8-42
    //0A-00-44-00-00-04-50-43-54-00
    //08-00-44-00-00-05-00-10
    //0A-00-44-00-00-80-01-00-01-00
    //11-00-44-00-01-00-53-50-45-45-44-00-00-00-00-00-00
    //0E-00-44-00-01-01-00-00-C8-C2-00-00-C8-42
    //0E-00-44-00-01-02-00-00-C8-C2-00-00-C8-42
    //0E-00-44-00-01-03-00-00-C8-C2-00-00-C8-42
    //0A-00-44-00-01-04-50-43-54-00
    //08-00-44-00-01-05-10-10
    //0A-00-44-00-01-80-01-00-04-00
    //11-00-44-00-02-00-50-4F-53-00-00-00-00-00-00-00-00
    //0E-00-44-00-02-01-00-00-B4-C3-00-00-B4-43
    //0E-00-44-00-02-02-00-00-C8-C2-00-00-C8-42
    //0E-00-44-00-02-03-00-00-B4-C3-00-00-B4-43
    //0A-00-44-00-02-04-44-45-47-00
    //08-00-44-00-02-05-08-08
    //0A-00-44-00-02-80-01-02-04-00
    //0B-00-43-00-01-0F-03-06-00-07-00
    //0B-00-43-01-01-0F-03-06-00-07-00
    //0B-00-43-02-01-07-0B-5F-06-A0-00
    //05-00-04-03-00
    //05-00-04-02-00
    //06-00-01-02-06-02
    //-> 06-00-01-02-06-00
    //-> 06-00-01-02-06-01
    //-> 0B-00-43-00-01-0F-03-06-00-07-00
    //-> 0B-00-43-01-01-0F-03-06-00-07-00
    //-> 05-00-05-21-06
    //-> 05-00-05-21-06
    //-> 0B-00-43-10-01-07-03-06-00-07-00
    //-> 0B-00-43-32-01-01-02-00-00-03-00
    //-> 0B-00-43-3A-01-06-08-FF-00-00-00
    //-> 0B-00-43-3B-01-02-02-03-00-00-00
    //-> 0B-00-43-3C-01-02-02-03-00-00-00
    //-> 0B-00-43-46-01-04-03-00-00-00-00
    //-> 07-00-43-00-02-06-00
    //-> 07-00-43-01-02-06-00
    //-> 05-00-05-21-06
    //-> 05-00-05-21-06
    //-> 07-00-43-10-02-06-00
    //-> 05-00-43-32-02
    //-> 07-00-43-3A-02-1F-00
    //-> 05-00-43-3B-02
    //-> 05-00-43-3C-02
    //-> 07-00-43-46-02-07-00";

    //    string toDecode = @"<- 06-00-22-46-00-00
    //-> 0D-00-01-01-06-4D-6F-76-65-20-48-75-62
    //<- 06-00-22-46-00-01
    //-> 11-00-44-46-00-00-54-52-49-47-47-45-52-00-00-00-00
    //-> 0E-00-44-46-00-01-00-00-00-00-00-00-20-41
    //<- 06-00-22-46-00-02
    //-> 0E-00-44-46-00-02-00-00-00-00-00-00-C8-42
    //<- 06-00-22-46-00-03
    //-> 0E-00-44-46-00-03-00-00-00-00-00-00-20-41
    //<- 06-00-22-46-00-04
    //-> 0A-00-44-46-00-04-00-00-00-00
    //<- 06-00-22-46-00-05
    //<- 06-00-22-46-00-06
    //-> 08-00-44-46-00-05-00-00
    //<- 06-00-22-46-00-07
    //-> 05-00-05-22-05
    //-> 05-00-05-22-05
    //<- 06-00-22-46-00-08
    //-> 05-00-05-22-05
    //<- 06-00-22-46-00-80
    //-> 0A-00-44-46-00-80-01-00-01-00
    //<- 06-00-22-46-01-00
    //-> 11-00-44-46-01-00-43-41-4E-56-41-53-00-00-00-00-00
    //<- 06-00-22-46-01-01
    //-> 0E-00-44-46-01-01-00-00-00-00-00-00-20-41
    //<- 06-00-22-46-01-02
    //-> 0E-00-44-46-01-02-00-00-00-00-00-00-C8-42
    //<- 06-00-22-46-01-03
    //-> 0E-00-44-46-01-03-00-00-00-00-00-00-20-41
    //<- 06-00-22-46-01-04
    //-> 0A-00-44-46-01-04-00-00-00-00
    //<- 06-00-22-46-01-05
    //-> 08-00-44-46-01-05-00-00
    //<- 06-00-22-46-01-06
    //-> 05-00-05-22-05
    //<- 06-00-22-46-01-07
    //-> 05-00-05-22-05
    //<- 06-00-22-46-01-08
    //-> 05-00-05-22-05
    //<- 06-00-22-46-01-80
    //-> 0A-00-44-46-01-80-01-00-01-00
    //<- 06-00-22-46-02-00
    //-> 11-00-44-46-02-00-56-41-52-00-00-00-00-00-00-00-00
    //<- 06-00-22-46-02-01
    //-> 0E-00-44-46-02-01-00-00-00-00-00-00-20-41
    //<- 06-00-22-46-02-02
    //-> 0E-00-44-46-02-02-00-00-00-00-00-00-C8-42
    //<- 06-00-22-46-02-03
    //-> 0E-00-44-46-02-03-00-00-00-00-00-00-20-41
    //<- 06-00-22-46-02-04
    //-> 0A-00-44-46-02-04-00-00-00-00
    //<- 06-00-22-46-02-05
    //-> 08-00-44-46-02-05-00-00
    //<- 06-00-22-46-02-06
    //-> 05-00-05-22-05
    //<- 06-00-22-46-02-07
    //-> 05-00-05-22-05
    //<- 06-00-22-46-02-08
    //-> 05-00-05-22-05
    //<- 06-00-22-46-02-80
    //-> 0A-00-44-46-02-80-01-02-01-00";

    //Handset announcement
    //    string toDecode = @"-> 0F-00-04-00-01-37-00-00-00-00-10-00-00-00-10
    //-> 0F-00-04-01-01-37-00-00-00-00-10-00-00-00-10
    //-> 0F-00-04-34-01-17-00-00-00-00-10-00-00-00-10
    //-> 0F-00-04-3B-01-14-00-02-00-00-00-02-00-00-00
    //-> 0F-00-04-3C-01-38-00-00-00-00-10-00-00-00-10";

    // Movehub announcement
    string toDecode = @"<- 0F-00-04-00-01-27-00-00-00-00-10-00-00-00-10
<- 0F-00-04-01-01-27-00-00-00-00-10-00-00-00-10
<- 0F-00-04-02-01-25-00-00-00-00-10-00-00-00-10
<- 0F-00-04-03-01-26-00-00-00-00-10-00-00-00-10
<- 09-00-04-10-02-27-00-00-01
<- 0F-00-04-32-01-17-00-00-00-00-01-06-00-00-20
<- 0F-00-04-3A-01-28-00-00-00-00-10-00-00-01-02
<- 0F-00-04-3B-01-15-00-02-00-00-00-00-00-01-00
<- 0F-00-04-3C-01-14-00-02-00-00-00-00-00-01-00
<- 0F-00-04-46-01-42-00-01-00-00-00-00-00-00-10";

    var lines = toDecode.Split("\n");
    foreach (var line in lines)
    {
        Console.WriteLine(line);
        string ln;
        if (line.IndexOf(' ') >= 0)
        {
            ln = line.Substring(line.IndexOf(' ') + 1).Trim('\r');
        }
        else
        {
            ln = line.Trim('\r');
        }

        var hexValues = ln.Split('-');
        byte[] byteArray = new byte[hexValues.Length];
        for (int i = 0; i < hexValues.Length; i++)
        {
            byteArray[i] = Convert.ToByte(hexValues[i], 16);
        }

        var msg = CommonMessageHeader.Decode(byteArray);
        Console.WriteLine(msg);
    }

    //PortModeDetails pmd = new PortModeDetails();
    //pmd.HubID = 0;
    //pmd.PortID = 0;
    //pmd.ModeIndex = 0;

    //foreach (byte[] element in elements)
    //{
    //    var msg = CommonMessageHeader.Decode(element);
    //    if (msg.MessageType == MessageType.PortModeInformation)
    //    {
    //        pmd.PopulateFromMessage((PortModeInformationMessage)msg);
    //    }
    //}

    //Console.WriteLine(pmd);

    //MoveHubInternalMotor motor = new MoveHubInternalMotor(new MockBluetooth(toDecode.Split("\r\n").ToList()), 0, 0);
    //foreach (byte[] element in elements)
    //{
    //    var msg = CommonMessageHeader.Decode(element);
    //    motor.PopulateModes(msg);
    //}

    //Console.WriteLine(motor.ToString());

    //var ble = new MockBluetooth(toDecode.Split("\r\n").ToList());
}

void DecodeByteArray()
{
    List<byte[]> elements = new List<byte[]>()
{
    new byte[] { 0x0B, 0x00, 0x43, 0x00, 0x01, 0x0F, 0x03, 0x06, 0x00, 0x07, 0x00                                                 },
    new byte[] { 0x07, 0x00, 0x43, 0x00, 0x02, 0x06, 0x00                                                                             },
    new byte[] { 0x11, 0x00, 0x44, 0x00, 0x00, 0x00, 0x50, 0x4F, 0x57, 0x45, 0x52, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00                 },
    new byte[] { 0x0E, 0x00, 0x44, 0x00, 0x00, 0x01, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42                                   },
    new byte[] { 0x0E, 0x00, 0x44, 0x00, 0x00, 0x02, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42                                   },
    new byte[] { 0x0E, 0x00, 0x44, 0x00, 0x00, 0x03, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42                                   },
    new byte[] { 0x0A, 0x00, 0x44, 0x00, 0x00, 0x04, 0x50, 0x43, 0x54, 0x00                                                           },
    new byte[] { 0x08, 0x00, 0x44, 0x00, 0x00, 0x05, 0x00, 0x10                                                                       },
    new byte[] { 0x0A, 0x00, 0x44, 0x00, 0x00, 0x80, 0x01, 0x00, 0x01, 0x00                                                           },
    new byte[] { 0x11, 0x00, 0x44, 0x00, 0x01, 0x00, 0x53, 0x50, 0x45, 0x45, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00                 },
    new byte[] { 0x0E, 0x00, 0x44, 0x00, 0x01, 0x01, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42                                   },
    new byte[] { 0x0E, 0x00, 0x44, 0x00, 0x01, 0x02, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42                                   },
    new byte[] { 0x0E, 0x00, 0x44, 0x00, 0x01, 0x03, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42                                   },
    new byte[] { 0x0A, 0x00, 0x44, 0x00, 0x01, 0x04, 0x50, 0x43, 0x54, 0x00                                                           },
    new byte[] { 0x08, 0x00, 0x44, 0x00, 0x01, 0x05, 0x10, 0x10                                                                       },
    new byte[] { 0x0A, 0x00, 0x44, 0x00, 0x01, 0x80, 0x01, 0x00, 0x04, 0x00                                                           },
    new byte[] { 0x11, 0x00, 0x44, 0x00, 0x02, 0x00, 0x50, 0x4F, 0x53, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00                 },
    new byte[] { 0x0E, 0x00, 0x44, 0x00, 0x02, 0x01, 0x00, 0x00, 0xB4, 0xC3, 0x00, 0x00, 0xB4, 0x43                                   },
    new byte[] { 0x0E, 0x00, 0x44, 0x00, 0x02, 0x02, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42                                   },
    new byte[] { 0x0E, 0x00, 0x44, 0x00, 0x02, 0x03, 0x00, 0x00, 0xB4, 0xC3, 0x00, 0x00, 0xB4, 0x43                                   },
    new byte[] { 0x0A, 0x00, 0x44, 0x00, 0x02, 0x04, 0x44, 0x45, 0x47, 0x00                                                           },
    new byte[] { 0x08, 0x00, 0x44, 0x00, 0x02, 0x05, 0x08, 0x08                                                                       },
    new byte[] { 0x0A, 0x00, 0x44, 0x00, 0x02, 0x80, 0x01, 0x02, 0x04, 0x00                                                           },
    new byte[] { 0x0B, 0x00, 0x43, 0x01, 0x01, 0x0F, 0x03, 0x06, 0x00, 0x07, 0x00                                                 },
    new byte[] { 0x07, 0x00, 0x43, 0x01, 0x02, 0x06, 0x00                                                                             },
    new byte[] { 0x11, 0x00, 0x44, 0x01, 0x00, 0x00, 0x50, 0x4F, 0x57, 0x45, 0x52, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00                 },
    new byte[] { 0x0E, 0x00, 0x44, 0x01, 0x00, 0x01, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42                                   },
    new byte[] { 0x0E, 0x00, 0x44, 0x01, 0x00, 0x02, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42                                   },
    new byte[] { 0x0E, 0x00, 0x44, 0x01, 0x00, 0x03, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42                                   },
    new byte[] { 0x0A, 0x00, 0x44, 0x01, 0x00, 0x04, 0x50, 0x43, 0x54, 0x00                                                           },
    new byte[] { 0x08, 0x00, 0x44, 0x01, 0x00, 0x05, 0x00, 0x10                                                                       },
    new byte[] { 0x0A, 0x00, 0x44, 0x01, 0x00, 0x80, 0x01, 0x00, 0x01, 0x00                                                           },
    new byte[] { 0x11, 0x00, 0x44, 0x01, 0x01, 0x00, 0x53, 0x50, 0x45, 0x45, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00                 },
    new byte[] { 0x0E, 0x00, 0x44, 0x01, 0x01, 0x01, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42                                   },
    new byte[] { 0x0E, 0x00, 0x44, 0x01, 0x01, 0x02, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42                                   },
    new byte[] { 0x0E, 0x00, 0x44, 0x01, 0x01, 0x03, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42                                   },
    new byte[] { 0x0A, 0x00, 0x44, 0x01, 0x01, 0x04, 0x50, 0x43, 0x54, 0x00                                                           },
    new byte[] { 0x08, 0x00, 0x44, 0x01, 0x01, 0x05, 0x10, 0x10                                                                       },
    new byte[] { 0x0A, 0x00, 0x44, 0x01, 0x01, 0x80, 0x01, 0x00, 0x04, 0x00                                                           },
    new byte[] { 0x11, 0x00, 0x44, 0x01, 0x02, 0x00, 0x50, 0x4F, 0x53, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00                 },
    new byte[] { 0x0E, 0x00, 0x44, 0x01, 0x02, 0x01, 0x00, 0x00, 0xB4, 0xC3, 0x00, 0x00, 0xB4, 0x43                                   },
    new byte[] { 0x0E, 0x00, 0x44, 0x01, 0x02, 0x02, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42                                   },
    new byte[] { 0x0E, 0x00, 0x44, 0x01, 0x02, 0x03, 0x00, 0x00, 0xB4, 0xC3, 0x00, 0x00, 0xB4, 0x43                                   },
    new byte[] { 0x0A, 0x00, 0x44, 0x01, 0x02, 0x04, 0x44, 0x45, 0x47, 0x00                                                           },
    new byte[] { 0x08, 0x00, 0x44, 0x01, 0x02, 0x05, 0x08, 0x08                                                                       },
    new byte[] { 0x0A, 0x00, 0x44, 0x01, 0x02, 0x80, 0x01, 0x02, 0x04, 0x00                                                           },
};

    var ble = new MockBluetooth(elements);
    MoveHub moveHub = new MoveHub(ble);
    ble.ProcessIncoming = moveHub.ProcessReceived;

    ble.Connect();

    Console.WriteLine(moveHub.ToString());
}

void HydrateForSerialization()
{
    List<byte[]> elements = new List<byte[]>();
    var ble = new MockBluetooth(elements);
    MoveHub moveHub = new MoveHub(ble);
    ble.ProcessIncoming = moveHub.ProcessReceived;

    foreach (BaseDevice device in moveHub.Devices)
    {
        var conf = device.GetDefaultConfiguration();
        if (conf != null)
        {
            foreach (byte[] elm in conf)
            {
                elements.Add(elm);
            }
        }
    }

    ble.Connect();

    foreach (BaseDevice device in moveHub.Devices)
    {
        Console.WriteLine($"Device: {device.ToString()}");
        foreach (PortModeDetails details in device.Modes)
        {
            Console.WriteLine($"Name: {details.Name}");
            Console.WriteLine($"Raw: {details.RawMin} {details.RawMax}");
            Console.WriteLine($"Pct: {details.PctMin} {details.PctMax}");
            Console.WriteLine($"SI: {details.SiMin} {details.SiMax}");
        }
    }
}
