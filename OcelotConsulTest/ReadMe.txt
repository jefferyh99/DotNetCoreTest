启动一个agent：consul agent -dev 测试使用

说明：
consul agent -server -datacenter=dc1 -bootstrap -data-dir ./data -config-file ./conf -ui-dir ./dist -node=n1 -bind 本机IP -client 0.0.0.0

URL地址有问题
consul agent -server -datacenter=dc1 -bootstrap -data-dir ./data -config-file ./config -ui-dir ./dist -node=n1 -bind 0.0.0.0 -client 0.0.0.0

没有URL地址
consul agent -server -datacenter=dc1 -bootstrap -data-dir ./data -config-file ./config -ui -node=n1 -bind 0.0.0.0 -client 0.0.0.0

添加期待的节点数
consul agent -server -datacenter=dc1 -bootstrap-expect 3 -data-dir ./data -config-file ./config -ui -node=n1 -bind 0.0.0.0 -client 0.0.0.0

说明：
运行cosnul agent以server模式，
-server ： 定义agent运行在server模式
-bootstrap-expect ：在一个datacenter中期望提供的server节点数目，当该值提供的时候，consul一直等到达到指定sever数目的时候才会引导整个集群，该标记不能和bootstrap共用
-bind：该地址用来在集群内部的通讯，集群内的所有节点到地址都必须是可达的，默认是0.0.0.0
-node：节点在集群中的名称，在一个集群中必须是唯一的，默认是该节点的主机名
-ui-dir： 提供存放web ui资源的路径，该目录必须是可读的
-rejoin：使consul忽略先前的离开，在再次启动后仍旧尝试加入集群中。
-config-dir：配置文件目录，里面所有以.json结尾的文件都会被加载
-client：consul服务侦听地址，这个地址提供HTTP、DNS、RPC等服务，默认是127.0.0.1所以不对外提供服务，如果你要对外提供服务改成0.0.0.0

参考文档：https://www.cnblogs.com/axzxs2001/p/8487521.html