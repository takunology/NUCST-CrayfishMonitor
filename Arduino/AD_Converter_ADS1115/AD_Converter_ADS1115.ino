#include <Adafruit_ADS1X15.h>

Adafruit_ADS1115 ads;

void setup() {
  Serial.begin(9600);
  
   ads.setGain(GAIN_TWOTHIRDS);  // 2/3x gain +/- 6.144V  1 bit = 0.1875mV (default)
  // ads.setGain(GAIN_ONE);        // 1x gain   +/- 4.096V  1 bit = 0.125mV
  // ads.setGain(GAIN_TWO);        // 2x gain   +/- 2.048V  1 bit = 0.0625mV
  if (!ads.begin()) {
    Serial.println("Failed to initialize ADS.");
    while (1);
  }
}

void loop() {
  int16_t input_0 = 0;
  float voltage = 0;

  //平均化するか？
  /*for (int i = 0; i < 5; i++){
    input_0 = ads.readADC_SingleEnded(0);
    voltage += ads.computeVolts(input_0);
  }
  
  voltage = voltage / 10;
  */
  input_0 = ads.readADC_SingleEnded(0);
  voltage += ads.computeVolts(input_0);
  //Serial.print(input_0);
  //Serial.print("\t");
  Serial.println(String(voltage, 5));
  
  //delay(5);
}
