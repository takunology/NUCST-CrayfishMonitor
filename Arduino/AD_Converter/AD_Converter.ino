void setup() {
  // put your setup code here, to run once:
  pinMode(13, OUTPUT );
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  int analogVal = 0;
  digitalWrite(13, HIGH);
  
  analogVal = analogRead(A0);
  float digitalVal = (float)(5 * analogVal)/1024;
  Serial.print(analogVal);
  Serial.print(",");
  Serial.println(String(digitalVal, 3));
  
  delay(10);
}
