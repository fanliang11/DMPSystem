# DMPSystem
分布式管理系统， 包括日志收集，消息订阅，SOA,集群管理，性能数据监控
配置文件 Event.Bindings.config
&lt;eventProvider&gt;
  &lt;queues&gt;
    &lt;queue id=&quot;DMPHubEvent&quot; class=&quot;DMPSystem.Core.EventBus.Utilities.QueueContext,DMPSystem.Core.EventBus&quot;&gt;
      &lt;property name=&quot;appRuleFile&quot; ref=&quot;rule&quot;/&gt;
      &lt;property name=&quot;dataContextPool&quot; value=&quot;event_sample&quot;&gt;
        &lt;map name=&quot;RabbitMq&quot;&gt;
          &lt;property  value=&quot;guest:guest@127.0.0.1::test&quot;/&gt;
        &lt;/map&gt;
        &lt;map name=&quot;MsMq&quot;&gt;&lt;/map&gt;
        &lt;map name=&quot;PushEvent&quot;&gt;
        &lt;/map&gt;
      &lt;/property&gt;
      &lt;property name=&quot;connectTimeout&quot; value=&quot;120&quot;/&gt;
      &lt;property name=&quot;CunsumerNum&quot;  value=&quot;5&quot;/&gt;
      &lt;property name=&quot;UseRetryNum&quot;  value=&quot;5&quot;/&gt;

    &lt;/queue&gt;
  &lt;/queues&gt;
&lt;/eventProvider&gt;

push publisher:

EventContainer.GetInstances&lt;IEventPublisher&gt;(&quot;PushEvent&quot;).Publish(new ChanageStateEvent() { UserID = manager.UserID });</br>
rabbitmq publisher:
    _eventPublisher = EventContainer.GetInstances&lt;IEventPublisher&gt;(&quot;DMPHubEvent.RabbitMq&quot;).Publish(new ChanageStateEvent() { UserID = manager.UserID });
