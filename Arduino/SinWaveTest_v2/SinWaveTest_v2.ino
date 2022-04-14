#define Pi 3.141592653589793

uint32_t time = 0;
uint32_t elapsed = 0;
String send_value = "";
int value_count = 0;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
}

void loop() {
  double val = 0.0;

  for (int deg = 0; deg < 360; deg++) {
    value_count++;
    double rad = deg / (180 / Pi);
    val = sin(rad) + 1.0f;      // 計測値
    elapsed = millis() - time;  // 計測時間
    send_value += String(elapsed) + "," + String(val) + "\n";

    // ある程度データをまとめて送る
    if (value_count == 100) {
      Serial.print(send_value);
      send_value = "";
      value_count = 0;
    }

    //Serial.print(send_value);
    /*Serial.print(value_count);
    Serial.print(" ");
    Serial.print(elapsed);
    Serial.print(" ");
    Serial.println(val);*/
    delay(10);
    //delayMicroseconds(10);
  }
}
