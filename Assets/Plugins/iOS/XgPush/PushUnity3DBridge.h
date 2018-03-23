//
//  PushUnity3DBridge.h
//
//  Created by houkai on 2018/3/22.
//  Copyright © 2018年 houkai. All rights reserved.
//

#ifndef PushUnity3DBridge_h
#define PushUnity3DBridge_h

#if defined (__cplusplus)
extern "C" {
#endif
    
    /**
     *	@brief	初始化
     *
     *	@param 	gameObject      U3D游戏对象名
     *  @param  callback        回调函数名
     */
    extern void __PushUnity3DBridge_init (void *gameObject, void*callback);
    
    /**
     *	@brief	是否开启调试
     *
     *	@param 	enable          U3D游戏对象名
     */
    extern void __PushUnity3DBridge_enableDebug(bool enable);
    
    /**
     *	@brief	获取本机Token
     *
     */
    extern const char*  __PushUnity3DBridge_getToken();
    
    /**
     *	@brief	开启推送服务
     *
     *	@param 	appId          appid
     *	@param 	appKey         appKey
     */
    extern void __PushUnity3DBridge_start(long appId, void*appKey);
    
    /**
     *	@brief	停止推送服务
     *
     */
    extern void __PushUnity3DBridge_stop();
    
    /**
     *	@brief	绑定账户
     *
     *	@param 	account        账户名
     */
    extern void __PushUnity3DBridge_bindAccount(void* account);
    
    /**
     *	@brief	解绑账户
     *
     *	@param 	account        账户名
     */
    extern void __PushUnity3DBridge_unbindAccount(void* account);
    
    /**
     *	@brief	绑定标签
     *
     *	@param 	tag            标签
     */
    extern void __PushUnity3DBridge_bindTag(void* tag);
    
    /**
     *	@brief	解绑标签
     *
     *	@param 	tag            标签
     */
    extern void __PushUnity3DBridge_unbindTag(void* tag);

#if defined (__cplusplus)
}
#endif
        
#endif /* PushUnity3DBridge_h */
