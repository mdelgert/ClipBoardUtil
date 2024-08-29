// Config.h
#ifndef CONFIG_H
#define CONFIG_H

#include <Arduino.h>

struct Config {
  struct {
    String ssid;
    String password;
  } wifi;

  struct {
    String broker;
    int port;
    String topic;
    String user;
    String password;
    String certificate;
  } mqtt;

  struct {
    String name;
    bool jiggler;
    bool setup_mode;
    bool keyboard_enable;
  } device;

  struct {
    String secret_1;
    String secret_2;
    String secret_3;
    String secret_4;
    String secret_5;
    String secret_6;
    String secret_7;
    String secret_8;
    String secret_9;
    String secret_10;
  } user_secrets, device_secrets;
};

#endif
