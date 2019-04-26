# Unity-ObjectPooling

![](https://github.com/KorStrix/Unity_ObjectPooling/blob/master/Images_ForGitHub/Preview.gif?raw=true)

유니티에서 사용하는 오브젝트 풀링 코드입니다.

## 주의사항
유니티 `2017 ~ 2018버젼까지 동작 확인`하였으며,

**유니티 5버젼 이하는 Assembly Definition을 지원하지 않아 정상동작하지 않을 수 있습니다.**

**이 코드를 사용하시려면 Player Setting의 Scripting Runtime Version을 .NET 4.6으로 올리셔야 합니다.**

Test 코드로 인하여 ``Assembly Defintion``을 사용했습니다.

![](https://github.com/KorStrix/Unity_ObjectPooling/blob/master/Images_ForGitHub/RequireSetting.png?raw=true)

## 기능들
크게 3가지 풀링 매니져가 존재하며, 각각 예제가 있습니다.

#### 공통기능
- 하이어라키 윈도우에 현재 생성된 오브젝트와 사용 현황이 표기됩니다.
- 미리 오브젝트를 생성해놓을 수 있습니다.
- **풀링 매니져에 리턴이 되야만 사용 요청 때 풀링이 진행됩니다.**
  - 모든 사용한 오브젝트를 한번에 풀링 매니져에 리턴할 수 있습니다.

#### 1. 아무 클래스를 상속받지 않은 클래스에 대한 풀링. [(예제 링크)](https://github.com/KorStrix/Unity_ObjectPooling/tree/master/Assets/Pooling/Example/01.Pooling_NormalClass)
- 사용 후 수동으로 리턴해야 풀링 매니져에 리턴이 됩니다.

#### 2. 컴포넌트 클래스를 상속받은 클래스에 대한 풀링. [(예제 링크)](https://github.com/KorStrix/Unity_ObjectPooling/tree/master/Assets/Pooling/Example/02.Pooling_ComponentClass)
- 사용 후 수동으로 리턴하거나, 게임오브젝트가 DeActive가 되면 자동으로 풀링 매니져에 리턴이 됩니다.

#### 3. 컴포넌트 클래스를 상속받으며, Resources 폴더 안에 들어가있는 오브젝트에 대한 풀링. [(예제 링크)](https://github.com/KorStrix/Unity_ObjectPooling/tree/master/Assets/Pooling/Example/03.Pooling_InResource)
- 사용 후 수동으로 리턴해야 풀링 매니져에 리턴이 됩니다.
- Init때 지정한 Resource 내에 특정 경로 안에 있는 모든 오브젝트는 자동으로 풀링 대상이 됩니다.

## 연락처
유니티 개발자 모임 카카오톡 & 디스코드 링크입니다.

- 카카오톡 : https://open.kakao.com/o/gOi17az
- 디스코드 : https://discord.gg/9BYFEbG
