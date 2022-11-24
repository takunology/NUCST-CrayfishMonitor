#include <Adafruit_ADS1X15.h>

Adafruit_ADS1115 ads;
uint32_t time = 0;

void setup() {
  Serial.begin(9600);
  ads.setGain(GAIN_TWOTHIRDS);

  if (!ads.begin()) {
    Serial.println("Failed to initialize ADS.");
    while (1);
  }
}

void loop() {
  int16_t input_0 = 0;
  float voltage = 0;
  int count = 0;

  uint32_t elapsed = millis() - time; // 計測時間
  //for(count = 0; count < 5; count++)
  {
    input_0 = ads.readADC_SingleEnded(0);
    voltage = ads.computeVolts(input_0);
  }
  //voltage = voltage / count; //平均化
  String send_value = String(elapsed) + "," + String(voltage, 9);
  Serial.println(send_value);
  //delay(1);
}
