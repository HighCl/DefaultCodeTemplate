# Default Code Template
이 레포지토리는 강성욱(HighCl)의 기본 코드 템플릿입니다.

제가 게임을 개발하면서 쌓아온 노하우를 템플릿화 해놨으며<br>
UPM(Unity Package Manager)를 이용해 다운받을 수 있습니다.
<br><br>

# 블로그
[네이버 블로그](https://blog.naver.com/fdsa1469/221353170686)<br>
블로그에 제작중인 프로젝트의 정보와 회고 등의 활동들을 업로드하고 있습니다.
<br><br>

# 출시한 게임들
### Hellbound Roeps
[스팀](https://store.steampowered.com/app/2591090/Hellbound_Ropes/)<br>
[스토브](https://store.onstove.com/ko/games/2657)<br>
OP.GG for Desktop(WebGL Demo): OPGG 공식 사이트에서 다운받으면 이용할 수 있습니다.

### 길드 판타지
[플레이 스토어](https://play.google.com/store/apps/details?id=com.GameCell.GuildFantasy)


### 클릭해! 용사
[플레이 스토어](https://play.google.com/store/apps/details?id=com.jaarts.clickerhero)

### 전체 프로젝트 목록
[블로그](https://blog.naver.com/fdsa1469/223041499127)
<br><br>

# Import Setup
UPM을 이용해 Import가 가능합니다.

Package Manager -> [Add package from git URL...]에서<br>
```jsonc
https://github.com/HighCl/DefaultCodeTemplate.git?path=Assets#upm
```
위 링크를 입력하거나

프로젝트의 패키지 폴더로 이동하여 manifest.json파일을 오픈한 다음 dependencies에 이 패키지를 추가하면 이용 가능합니다.

```json
{
  "dependencies": {
    "com.highcl.code-template": "https://github.com/HighCl/DefaultCodeTemplate.git?path=Assets#upm",
    ...
  }
}
```
이후 Package Manager -> Samples에서 프로젝트를 Import하시면 됩니다.
<br><br>

# 유니티 버전
22.3.34f1
<br><br>

# 커밋 규칙
[Git 사용 규칙 - Git commit 메시지](https://tttsss77.tistory.com/58)<br>
제 프로젝트에서는 위 글의 커밋 컨벤션을 따르고 있습니다.
<br><br>

# 에셋
### 기본 설치 에셋
1. Text Mesh Pro Package<br>
비용: 무료(유니티 기본 패키지)<br>
비고: 종속성 설정이 되어 있어 3.0.6 버전 자동 설치

2. Input System Package<br>
비용: 무료(유니티 기본 패키지)<br>
비고: 종속성 설정이 되어 있어 1.7.0 버전 자동 설치

3. unity-excel-importer<br>
링크: [Git 링크](https://github.com/mikito/unity-excel-importer)<br>
라이센스: MIT License<br>
비고: 사용 편의성을 위한 수정 사항 일부 있음

### 설치 필요 에셋
1. ReadOnlyAttribute<br>
링크: [에셋 스토어 링크](https://assetstore.unity.com/packages/tools/gui/readonly-attribute-134710)<br>
비용: 무료

2. Inspector Button Pro - Editor Buttons With Parameters<br>
링크: [에셋 스토어 링크](https://assetstore.unity.com/packages/tools/utilities/inspector-button-pro-editor-buttons-with-parameters-151474)<br>
비용: 무료

위 에셋을 설치하지 않으면 <b>정상 작동하지 않습니다.</b><br>
<br>

# 기본 설정
- Editor - EnterPlayMode 설정<br>

※주의※ 실행 시 디버깅하지 않으므로 반드시 초기값 설정을 해줘야 합니다<br>
※주의※ 사용하는 에셋에 따라 Mode를 꺼줘야 할 수 있습니다.

- @Managers 프리팹의 _SettingManager에서 OnSetting 버튼 실행<br>
1. SteamSdk 사용 여부 설정<br>
2. 사용할 Input System 설정<br>
3. TMP 기본값 설정<br>
3-1. EnableRaycastTarget 설정<br>
3-2. 기본 폰트 변경(파일명으로 프로젝트 내부 SDF 폰트 에셋 검색)

- Editor Config 추가
```jsonc
root = true

[*]
charset = utf-8
```
프로젝트 폴더에 위 내용을 포함한 .editorconfig 파일을 만들어 추가합니다.