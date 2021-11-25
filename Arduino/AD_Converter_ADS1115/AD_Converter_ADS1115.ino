#include <Adafruit_ADS1X15.h>

Adafruit_ADS1115 ads;

void setup() {
  Serial.begin(9600);
  //+-4.096V
  ads.setGain(GAIN_ONE);
  if (!ads.begin()) {
    Serial.println("Failed to initialize ADS.");
    while (1);
  }
}

void loop() {
  int16_t input_0;
  double voltage;
  
  input_0 = ads.readADC_Differential_0_1();

  //+-4.096Vのときは0.125 / +-6.144Vのとき0.1875
  voltage = (double)(input_0 * 0.125)/1000;
  Serial.println(String(voltage, 5));
  
  /*Serial.print(input_0);
  Serial.print(" ");
  Serial.println(voltage);*/
  delay(5);
}
