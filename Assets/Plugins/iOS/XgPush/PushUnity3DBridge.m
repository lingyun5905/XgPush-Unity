//
//  PushUnity3DBridge.m
//
//  Created by houkai on 2018/3/22.
//  Copyright © 2018年 houkai. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "PushUnity3DBridge.h"
#import "XGPush.h"

#if __IPHONE_OS_VERSION_MAX_ALLOWED >= __IPHONE_10_0
#import <UserNotifications/UserNotifications.h>
#endif

@interface PushUnity3DBridge : NSObject<XGPushDelegate> {
@private
    NSString* _gameObject;
    NSString* _callback;
}
-(void) init:(NSString *)gameobject callback:(NSString *)callback;
-(NSString*) getMethodJson:(NSString *)methodName success:(BOOL)isSuccess msg:(NSString *)msg;
+(instancetype) instance;
@end

char * __StringCopy( const char *string)
{
    if (string != NULL)
    {
        char *copyStr = (char*)malloc(strlen(string)+1);
        strcpy(copyStr, string);
        return copyStr;
    }
    else
    {
        return NULL;
    }
}

#if defined (__cplusplus)
extern "C" {
#endif
    
    extern void __PushUnity3DBridge_init (void *gameObject, void*callback){
        NSString *nsGO = [NSString stringWithCString:gameObject encoding:NSUTF8StringEncoding];
        NSString *nsCB = [NSString stringWithCString:callback encoding:NSUTF8StringEncoding];
        NSLog(@"gameObject:%@  callback:%@", nsGO, nsCB);
        [[PushUnity3DBridge instance] init:nsGO callback:nsCB];
    }
    
    extern void __PushUnity3DBridge_enableDebug(bool enable){
        [[XGPush defaultManager] setEnableDebug:enable];
    }
    
    extern const char*  __PushUnity3DBridge_getToken(){
        NSString* token = [[XGPushTokenManager defaultTokenManager] deviceTokenString];
        return __StringCopy([token UTF8String]);
    }
    
    extern void __PushUnity3DBridge_start(long appId, void*appKey){
        NSString *nsAppKey = [NSString stringWithCString:appKey encoding:NSUTF8StringEncoding];
        [[XGPush defaultManager] startXGWithAppID:(uint32_t)appId appKey:nsAppKey delegate:[PushUnity3DBridge instance]];
    }
    
    extern void __PushUnity3DBridge_stop(){
        [[XGPush defaultManager] stopXGNotification];
    }
    
    extern void __PushUnity3DBridge_bindAccount(void* account){
        NSString *nsAccount = [NSString stringWithCString:account encoding:NSUTF8StringEncoding];
        [[XGPushTokenManager defaultTokenManager] bindWithIdentifier:nsAccount type:XGPushTokenBindTypeAccount];
    }
    
    extern void __PushUnity3DBridge_unbindAccount(void* account){
        NSString *nsAccount = [NSString stringWithCString:account encoding:NSUTF8StringEncoding];
        [[XGPushTokenManager defaultTokenManager] unbindWithIdentifer:nsAccount type:XGPushTokenBindTypeAccount];
    }
    
    extern void __PushUnity3DBridge_bindTag(void* tag){
        NSString *nsTag = [NSString stringWithCString:tag encoding:NSUTF8StringEncoding];
        [[XGPushTokenManager defaultTokenManager] bindWithIdentifier:nsTag type:XGPushTokenBindTypeTag];
    }
    
    extern void __PushUnity3DBridge_unbindTag(void* tag){
        NSString *nsTag = [NSString stringWithCString:tag encoding:NSUTF8StringEncoding];
        [[XGPushTokenManager defaultTokenManager] unbindWithIdentifer:nsTag type:XGPushTokenBindTypeTag];
    }
    
#if defined (__cplusplus)
}
#endif


@implementation PushUnity3DBridge

+(instancetype) instance {
    static dispatch_once_t once;
    static PushUnity3DBridge *instance;
    dispatch_once(&once, ^{
        instance = [[self.class alloc] init];
    });
    return instance;
}

-(void) init:(NSString *)gameobject callback:(NSString *)callback {
    self->_gameObject = gameobject;
    self->_callback = callback;
}

-(NSString*) getMethodJson:(NSString *)methodName success:(BOOL)isSuccess msg:(NSString *)msg{
    NSMutableDictionary *dict = [NSMutableDictionary dictionaryWithCapacity:3];
    [dict setValue:methodName forKey:@"method"];
    [dict setValue:@(isSuccess) forKey:@"success"];
    if (msg!= nil) {
        [dict setObject:msg forKey:@"msg"];
    }
    NSError *e = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict options:NSJSONWritingPrettyPrinted error:&e];
    if (e != nil || jsonData == nil) {
        NSLog(@"getMethodJson json parse error! %@", e);
        return @"";
    }
    NSString *jsonStr = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    return jsonStr;
}

#pragma mark - XGPushDelegate
- (void)xgPushDidFinishStart:(BOOL)isSuccess error:(NSError *)error {
    NSLog(@"%s, result %@, error %@", __FUNCTION__, isSuccess?@"OK":@"NO", error);
    
    NSString* msg = nil;
    if (error != NULL) {
        msg = [error localizedDescription];
    }
    NSString *jsonStr = [self getMethodJson:@"OnStartFinish" success:isSuccess msg:msg];
    UnitySendMessage([self->_gameObject UTF8String], [self->_callback UTF8String], [jsonStr UTF8String]);
}

- (void)xgPushDidFinishStop:(BOOL)isSuccess error:(NSError *)error {
    NSLog(@"%s, result %@, error %@", __FUNCTION__, isSuccess?@"OK":@"NO", error);
    
    NSString* msg = nil;
    if (error != NULL) {
        msg = [error localizedDescription];
    }
    NSString *jsonStr = [self getMethodJson:@"OnStopFinish" success:isSuccess msg:msg];
    UnitySendMessage([self->_gameObject UTF8String], [self->_callback UTF8String], [jsonStr UTF8String]);
    
}

// iOS 10 新增 API
// iOS 10 会走新 API, iOS 10 以前会走到老 API
#if __IPHONE_OS_VERSION_MAX_ALLOWED >= __IPHONE_10_0
// App 用户点击通知
// App 用户选择通知中的行为
// App 用户在通知中心清除消息
// 无论本地推送还是远程推送都会走这个回调
- (void)xgPushUserNotificationCenter:(UNUserNotificationCenter *)center didReceiveNotificationResponse:(UNNotificationResponse *)response withCompletionHandler:(void (^)(void))completionHandler {
    [[XGPush defaultManager] reportXGNotificationResponse:response];
    
    completionHandler();
}

// App 在前台弹通知需要调用这个接口
- (void)xgPushUserNotificationCenter:(UNUserNotificationCenter *)center willPresentNotification:(UNNotification *)notification withCompletionHandler:(void (^)(UNNotificationPresentationOptions))completionHandler {
    [[XGPush defaultManager] reportXGNotificationInfo:notification.request.content.userInfo];
    completionHandler(UNNotificationPresentationOptionBadge | UNNotificationPresentationOptionSound | UNNotificationPresentationOptionAlert);
}
#endif


@end
