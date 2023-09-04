# CSharpBaseServer
[인프런의 Rookis-C# 게임서버 강의](https://www.inflearn.com/course/%EC%9C%A0%EB%8B%88%ED%8B%B0-mmorpg-%EA%B0%9C%EB%B0%9C-part4/dashboard)를 들으며 만든 C# Base Server

## 기간
* 2023년 7월 4일 ~ 2023년 9월 3일

## 참고자료
* 강의를 들으며 정리한 내용과 나의 생각은 **[Velog - Seoki.log](https://velog.io/@pkoi5088)** 에서 볼 수 있습니다.
* 블로그의 ServerTag나 SocketTag를 확인하면 쉽게 찾을 수 있습니다.

## 프로젝트 구조
### 1. ServerCore

클라이언트, 서버가 사용할 클래스들을 정의해 놓은 솔루션으로 다음과 같은 클래스들이 정의되어 있습니다. 프로젝트에 따라 사용하지 않아도 되는 클래스들이 있습니다.

* **Connector**: 클라이언트와 같은 세션연결요청을 하기 위한 호스트에서 사용하는 클래스로 연결하고자 하는 EndPoint와 Session생성 Func를 인자로 받습니다.

* **Listener**: 서버와 같은 세션연결요청에 대한 응답을 하기위한 호스트에서 사용하는 클래스로 Init함수를 통해 클라이언트의 연결을 Listen하는 클래스입니다.

* **RecvBuffer, SendBuffer**: Session에서 사용하는 버퍼를 구현한 클래스로 프로젝트에 따라 사용할 버퍼 사이즈를 조절할 수 있습니다.

* **Session**: 클라이언트, 서버가 사용할 Session 인터페이스로 Start, Send, Disconnect함수가 구현되어 있습니다. Start는 **'상대방으로부터 데이터를 받을 준비가 되어있습니다.'**라는 의미로 Receive를 담당하는 함수입니다.

### 2. Server
서버를 담당하는 솔루션으로 main함수는 Program.cs에 있습니다. 내부 게임 로직이나 PacketHandler, Session Event를 정의한 클래스가 있습니다.

* **Packet/**: Packet에 대한 클래스들이 존재하는 디렉토리로 PacketHandler에서 정의한 패킷에 대한 Action을 구현할 수 있습니다.

* **Session/**: Session에 대한 클래스들이 존재하는 디렉토리로 Session Event가 정의되어 있는 ClientSession, Session을 관리하는 Manager 클래스가 있습니다.

### 3. PacketGenerator

**'PDL.xml'** 에 정의되어 있는 패킷구조를 바탕으로 Server, Client에서 사용할 Packet관련 코드들을 생성하는 자동화 코드생성 솔루션입니다. `int`, `float`, `string`과 같은 변수타입과 `struct list`를 xml에서 정의할 수 있습니다. 필요에 따라서 추가적인 자료형을 Program.cs에서 추가할 수 있으며 생성할 FildFormat을 PacketFormat.cs에서 확인 및 수정할 수 있습니다.

### 4. DummyClient
ServerTest를 위한 솔루션으로 Server연결과 Event, Action의 간단한 구현이 존재합니다.

현재 프로젝트에 구현되어 있는 코드는 각각의 클라이언트가 무한루프로 랜덤위치에 이동패킷을 보내는 상황을 구현해놓은 것입니다.

### 5. Common
PacketGenerator에서 빌드한 exe파일을 실행할 bat파일이 존재하는 폴더입니다. 프로젝트에 따라 클라이언트, 서버에서 사용할 공용툴을 보관할 디렉토리입니다. GenPackets.bat이 실행할 bat파일이며 PacketGenerator를 실행결과를 Server, Client의 특정 위치에 이동하는 bat파일입니다.

### 6. Client
DummyClient를 유니티 프로젝트에 적용한 유니티 프로젝트로 Server를 구동하고 Client를 실행시키면 연결이되었다는 알림창이 출력됩니다. Client의 연결구조를 확인하려면 **'Assets/Scripts/.../NetworkManager'** 에서 확인할 수 있습니다.

## 사용방법

**1. 패킷설계**

패킷구조를 PDL.xml에 입력하고 PacketGenerator를 빌드를 하고 Common/Packet의 bat파일을 실행시키세요.

Server, Client의 Packet/PacketHandler에 PacketManager의 HandlerFunction을 정의하고 구현하시면됩니다.

Protobuf와 같은 외부 패킷직렬화 라이브러리를 사용하려면 Pakcet구조에 대한 코드는 사용하지 않아도 좋습니다.

**2. ServerCore 세팅**

Client, Server에서 사용할 ServerCore를 세팅하면 됩니다. Server의 같은 경우 ServerCore를 종속성설정을 하면 자동으로 포함되며 Client의 경우 ServerCore.dll을 사용하거나 Server와 같이 종속성설정을 하시면 됩니다.

**3. Session Event, Packet Handler 구현**

위에서 언급한 Session Event와 Pakcet Handler를 구현하면 됩니다. Packet Handler는 **'해당 패킷이 들어왔을 때 ~한 행동을 하겠습니다.'**를 의미합니다.

**4. 로직 구현**

Server와 Client의 내부로직을 구현하면 됩니다. 로직을 먼저 구현하고 Session Event, Packet Handler를 구현하는 것이 더 좋을 수 있습니다.
