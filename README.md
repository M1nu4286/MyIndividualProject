# MyIndividualProject


림버스 컴퍼니는 한 화면에 캐릭터가 최대 14명 등장하는 다객체 턴제 게임임으로 엔티티별 데이터를 각각의 객체가 따로 소유하는 방식이 아닌 (God Object 견제) CSV파일(EntityDatabase,SkillDatabase)을 데이터 피싱을 통한 SO에셋 배열로 저장하는 방식 사용
객체 별 스킬과 스텟 데이터는 ScriptableObject를 통해 Class로 관리 및 수시로 변하며 캐시 적중률이 높아야하는 런타임 데이터는 구조체Struct로 관리 
런타임 중 메모리 성능을 최대화하기 위해 객체 정보가 담겨진 배열은 수정X
대신 간접참조용 정수 배열을 선언하여 속도에 따른 우선순위 큐 방식으로 순서 지정

스크립트들은 최상위 관리자 BattleManager를 만들어서 DOD-데이터 지향 설계 방식으로 관리함
입력과 로직 분리 - 입력 즉시 연산을 실행하는 방식이 아닌 입력들을 버퍼에 저장한뒤 속도 우선순위에 따른 일괄 연산 실행

FSM 사용
    BattleStartState: 게임 실행 시 객체 데이터 로드 및 속도 리롤
    EvaluationState: 속도에 따른 객체 배치 , UI 표시 및 플레이어 조작 단계
    HapAndClashState: EvaluationState에서 받은 명령 버퍼를 일괄 연산
    ActionExecutionState: HapAndClashState의 연산 결과에 따른 애니메이션 재생
    CleanUpState: 사망 및 HP감소와 같은 객체 상태 최신화


CSV는 기본적으로 문자열이므로 String을 통한 EntityID-SkillID 간 ID대조가 아닌 Dictionary<int(hash값),int(value)>를 사용한 Hash-ID 대조 사용
CSV파일을 최신화할때마다 코드 수정을 하지 않도록 처음부터 길이를 정해놓는 배열이 아닌 리스트로 저장한뒤 배열로 변환
인스펙터 창에서 Hash값을 띄우는건 난해하기 때문에 전처리문을 사용해 문자열 ID표시


힘들었던 점:
처음엔 어떤상황에 Struct를 써야하는지 Class를 써야하는지, 또 List와 배열Array를 써야하는 상황이 많이 혼동됐다.
이론으로는 무슨 차이가 있었는지 알고있었지만 이를 게임을 만들면서 최적화를 진행할 때 적용하는 것은 또 다른 영역이었다.

처음 CSV파일을 통한 SO를 import할때 관련 지식이 부족해 난항을 겪었다. 이를 구현하는데 적지않은 시간이 들었던 것 같다.
최대한 유니티API를 사용하지 않고 C# 스크립트만을 사용해 로직을 구현하려 노력했다.

SO
최적화 방안
메모리 레이아웃을 고려하여 불필요한 참조 최소화

Importer
최적화 방안
GC(Garbage Collector) 최소화: 루프 내부에서 string.Split이나 Parse를 남발하는 것은 툴 단계에서는 허용되나, 데이터가 수만 개라면 StringBuilder나 Span<T>(C# 7.2+) 도입을 검토하라.

AssetDatabase.SaveAssets()의 비용: 수천 개의 SO 파일을 개별적으로 생성하는 것은 I/O 병목을 유발한다. 하나의 대용량 SO에 리스트 형태로 담는 것이 관리와 로딩 속도 면에서 효율적이다.

DOD (Data-Oriented Design): SO 내부에 클래스 객체 리스트를 두는 것은 포인터 추적(Pointer Chasing)을 발생시킨다. 성능이 중요하다면 필드별로 배열을 분리하는 방식을 고민해라.

int.TryParse사용 고려

후에 최적화 과정떄 ReadLine대신 StreamReader 사용 고려


상황,선택,로우레벨 이유
순수 데이터(Data-only),Struct,"CPU 캐시 히트율 극대화, GC 부하 제거"
기능/로직 중심(System),Class,"참조를 통한 상태 공유, 다형성 활용"
빈번한 생성/파괴,Struct,힙 할당 없이 스택에서 즉시 해제
복잡한 관계/참조 그래프,Class,객체 간의 주소 연결(Link) 용이성


구조체(값타입) 속에 string,배열 (참조타입) 넣지 말기 ->참조 오염 발생