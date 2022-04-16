#define Pi 3.141592653589793

uint32_t time = 0;
//String send_value = "";
int value_count = 0;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
}

void loop() {
  for (int deg = 0; deg < 360; deg++) {
    value_count++;
    double rad = deg / (180 / Pi);
    double val = sin(rad) + 1.00000f;      // 計測値
    uint32_t elapsed = millis() - time;  // 計測時間
    String send_value = String(elapsed) + "," + String(val);
    Serial.println(send_value);
    
    // ある程度データをまとめて送る場合
    /*if (value_count == 100) {
      Serial.println(send_value);
      send_value = "";
      value_count = 0;
    }*/

    //Serial.print(send_value);
    /*Serial.print(value_count);
    Serial.print(" ");
    Serial.print(elapsed);
    Serial.print(" ");
    Serial.println(val);*/
    delay(1);
    //delayMicroseconds(1);
  }
}
